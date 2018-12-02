using fNbt;
using codeshaper.buildings;
using codeshaper.data;
using codeshaper.entity.unit.stats;
using codeshaper.entity.unit.task;
using UnityEngine;
using codeshaper.entity.unit.task.attack;
using codeshaper.nbt;

namespace codeshaper.entity.unit {

    public abstract class UnitBase : SidedObjectEntity {

        private ITask task;
        public AttackBase attack;
        public MoveHelper moveHelper;

        private Vector3? overrideMovementDestination;
        private float overrideMovementStopDis;

        public UnitStats unitStats;
        /// <summary> The position of the unit during the last frame. </summary>
        private Vector3 lastPos;

        protected override void onAwake() {
            base.onAwake();

            this.attack = this.createAttackMethod();
            this.lastPos = this.transform.position;
        }

        protected override void onStart() {
            base.onStart();

            this.moveHelper = new MoveHelper(this);
            this.setTask(null);
        }

        public override void onUpdate() {
            base.onUpdate();

            if(this.overrideMovementDestination != null) {
                if(Vector3.Distance(this.getFootPos(), (Vector3)this.overrideMovementDestination) <= this.overrideMovementStopDis + 0.5f) {
                    this.overrideMovementDestination = null;
                }
            } else if (this.task != null) {
                bool continueExecuting = this.task.preform();
                if (!continueExecuting) {
                    this.setTask(null, true); // Set unit to idle.
                }
            }

            // Update stats.
            if(this.transform.position != this.lastPos) {
                this.unitStats.distanceWalked.increase(Vector3.Distance(this.transform.position, this.lastPos));
            }
            this.lastPos = this.transform.position;

            this.unitStats.timeAlive.increase(Time.deltaTime);
        }

        public override void drawDebug() {
            base.drawDebug();

            // Draw a debug arrow pointing forward.
            GLDebug.DrawLineArrow(this.getPos(), this.getPos() + this.transform.forward, 0.25f, 20, Color.blue, 0, true);
            this.moveHelper.drawDebug();

            if(this.overrideMovementDestination == null && this.task != null) {
                this.task.drawDebug();
            }
        }

        public override void onInitialConstruct() {
            base.onInitialConstruct();

            this.unitStats = new UnitStats(this.getData());
        }

        protected override void colorUnit() {
            this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = this.getTeam().getColor();
        }

        public override bool damage(MapObject dealer, int amount) {
            amount = Mathf.RoundToInt(amount * (this.unitStats.getDefense() / Constants.BASE_DEFENSE_VALUE));
            this.unitStats.damageTaken.increase(amount);
            bool killedByDamage = base.damage(dealer, amount);
            this.task.onDamage(dealer);
            return killedByDamage;
        }

        public override void onDeathCallback() {
            base.onDeathCallback();

            // Hacky way to remove unit from the party.
            CameraMover cm = CameraMover.instance();
            if(cm.getTeam() == this.getTeam()) {
                cm.selectedParty.remove(this);
            }

            GameObject.Destroy(this.gameObject);
        }

        public abstract EntityBaseStats getData();

        /// <summary>
        /// Sets the units task.  Pass null to set the current task to idle.
        /// </summary>
        public void setTask(ITask newTask, bool forceCancelPrevious = false) {
            // Explicitly setting a task while a unit is moving stops it's walk.
            this.overrideMovementDestination = null;

            if(this.task == null || (forceCancelPrevious || (this.task != null && this.task.cancelable()))) {
                // If there is an old task, call finish on the instance.
                if(this.task != null) {
                    this.task.onFinish();
                }

                this.task = (newTask == null) ? new TaskIdle(this) : newTask;
            }
        }

        /// <summary>
        /// Returns the exact position of the middle of the unit at it's feet.
        /// </summary>
        /// <returns></returns>
        public Vector3 getFootPos() {
            return this.transform.position + Vector3.down;
        }

        /// <summary>
        /// Returns the Unit's current task.  This will never be null.
        /// </summary>
        public ITask getTask() {
            return this.task;
        }

        /// <summary>
        /// Moves a unit to a certain point.  This overrides their current task as long as it is cancelable.
        /// </summary>
        public void walkToPoint(Vector3 point, int partySize) {
            if(this.task.cancelable()) {
                this.overrideMovementStopDis = partySize <= 1 ? 0f : (partySize <= 3 ? 1f : 3f); // TODO spread out stopping points.
                this.overrideMovementDestination = point;

                // Move the units to the destination, making sure they don't pile to close.
                this.moveHelper.setDestination(point, this.overrideMovementStopDis);
            }
        }

        public override float getSizeRadius() {
            return 0.5f;
        }

        public override int getMaxHealth() {
            return this.unitStats.getMaxHealth();
        }

        public virtual AttackBase createAttackMethod() {
            return new AttackMelee(this);
        }

        public override float getHealthBarHeight() {
            return 1.5f;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.lastPos = tag.getVector3("lastPos");
            this.unitStats = new UnitStats(tag, this.getData());

            bool flag = tag.getBool("hasMovementOverride");
            if(flag) {
                this.overrideMovementDestination = tag.getVector3("overrideMovementDestination");
                this.overrideMovementStopDis = tag.getFloat("overrideMovementStopDis");
            }
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("lastPos", this.lastPos);
            this.unitStats.writeToNBT(tag);

            tag.setTag("hasMovementOverride", this.overrideMovementDestination != null);
            if(this.overrideMovementDestination != null) {
                tag.setTag("overrideMovementDestination", (Vector3)this.overrideMovementDestination);
                tag.setTag("overrideMovementStopDis", this.overrideMovementStopDis);
            }
        }

        /// <summary>
        /// Damages the passed object and returns it.  Null will be returned if the object is destroyed.
        /// This method will also increase stats if needed.
        /// </summary>
        public SidedObjectEntity damageTarget(SidedObjectEntity obj) {
            int damage = this.unitStats.getAttack();
            this.unitStats.damageDelt.increase(damage);

            if(obj.damage(this, damage)) {
                // obj was killed.
                if (obj is BuildingBase) {
                    this.unitStats.buildingsDestroyed.increase();
                } else if (obj is UnitBase) {
                    this.unitStats.unitsKilled.increase();
                }
                return null;
            } else {
                return obj;
            }
        }
    }

    public abstract class UnitBase<T> : UnitBase where T : SidedObjectEntity { }
}
