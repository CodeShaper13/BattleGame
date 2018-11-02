using fNbt;
using codeshaper.buildings;
using codeshaper.data;
using codeshaper.entity.unit.stats;
using codeshaper.entity.unit.task;
using codeshaper.util;
using UnityEngine;
using codeshaper.entity.unit.task.attack;

namespace codeshaper.entity.unit {

    public abstract class UnitBase : SidedObjectEntity {

        private ITask task;
        public AttackBase attack;
        public MoveHelper moveHelper;
        public Plane[] planes;

        private Vector3? overrideMovementDestination;
        private float overrideMovementStopDis;

        public UnitStats unitStats;
        /// <summary> The position of the unit during the last frame. </summary>
        private Vector3 lastPos;

        protected override void onAwake() {
            base.onAwake();

            this.unitStats = new UnitStats(); //TODO don't generate all the stuff if this unit is being loaded from a file.
            this.moveHelper = new MoveHelper(this);
            this.lastPos = this.transform.position;

            this.attack = this.createAttackMethod();

            this.setTask(null);
        }

        protected override void onUpdate() {
            base.onUpdate();

            // Draw a debug arrow pointing forward.
            if(Main.DEBUG) {
                GLDebug.DrawLineArrow(this.getPos(), this.getPos() + this.transform.forward, 0.25f, 20, Color.blue, 0, true);
                this.moveHelper.drawDebug();
            }

            if(this.overrideMovementDestination != null) {
                if(Vector3.Distance(this.getPos(), (Vector3)this.overrideMovementDestination) <= this.overrideMovementStopDis + 1.25f) {
                    this.overrideMovementDestination = null;
                }
            } else if (this.task != null) {
                if(Main.DEBUG) {
                    this.task.drawDebug();
                }
                if (!this.task.preform()) {
                    this.setTask(null); // Set unit to idle.
                }
            }

            // Update stats.
            if(this.transform.position != this.lastPos) {
                this.unitStats.distanceWalked.increase(Vector3.Distance(this.transform.position, this.lastPos));
            }
            this.lastPos = this.transform.position;

            this.unitStats.timeAlive.increase(Time.deltaTime);
        }

        public override bool damage(MapObject dealer, int amount) {
            this.unitStats.damageTaken.increase(amount);
            bool flag = base.damage(dealer, amount);
            this.task.onDamage(dealer);
            return flag;
        }

        public override void onDeathCallback() {
            base.onDeathCallback();

            // Hacky way to remove unit from the party.
            CameraMover cm = CameraMover.instance();
            if(cm.getTeam() == this.getTeam()) {
                cm.party.remove(this);
            }

            GameObject.Destroy(this.gameObject);
        }

        public abstract EntityData getData();

        /// <summary>
        /// Sets the units task.  Pass null to set the current task to idle.
        /// </summary>
        public void setTask(ITask newTask) {
            // Explicitly setting a task while a unit is moving stops it's walk.
            this.overrideMovementDestination = null;

            if(this.task == null || (this.task != null && this.task.cancelable())) {
                // IF there is an old task, call finish on the instance.
                if(this.task != null) {
                    this.task.onFinish();
                }

                this.task = (newTask == null) ? new TaskIdle(this) : newTask;
            }
        }

        /// <summary>
        /// Returns the Unit's current task.  This will never be null.
        /// </summary>
        public ITask getTask() {
            return this.task;
        }

        /// <summary>
        /// Moves a unit to a certain point.  This overrides their current task.
        /// </summary>
        public void walkToPoint(Vector3 point, int partySize) {
            this.overrideMovementStopDis = partySize <= 1 ? 0f : (partySize <= 3 ? 1f : 3f); // TODO spread out stopping points.
            this.overrideMovementDestination = point;

            // Move the units to the destination, making sure they don't pile to close.
            this.moveHelper.setDestination(point, this.overrideMovementStopDis);
        }

        public override float getSizeRadius() {
            return 0.65f;
        }

        public override int getMaxHealth() {
            return this.getData().getHealth();
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
            this.unitStats.readFromNbt(tag);
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("lastPos", this.lastPos);
            this.unitStats.writeToNBT(tag);
        }

        /// <summary>
        /// Damages the passed object and returns it.  Null will be returned if the object is destroyed.
        /// This method will also increase stats if needed.
        /// </summary>
        public SidedObjectEntity damageTarget(SidedObjectEntity obj) {
            int damage = this.getData().getDamageDelt();
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
