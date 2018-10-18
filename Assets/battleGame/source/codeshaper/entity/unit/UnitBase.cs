using fNbt;
using codeshaper.buildings;
using codeshaper.data;
using codeshaper.entity.unit.stats;
using codeshaper.entity.unit.task;
using codeshaper.util;
using UnityEngine;
using UnityEngine.AI;

namespace codeshaper.entity.unit {

    public abstract class UnitBase : SidedObjectEntity {

        [HideInInspector]
        public NavMeshAgent agent;

        private ITask task;

        public UnitStats unitStats;
        /// <summary> The position of the unit during the last frame. </summary>
        private Vector3 lastPos;

        protected override void onAwake() {
            base.onAwake();
            this.agent = this.GetComponent<NavMeshAgent>();
            this.unitStats = new UnitStats();
            this.lastPos = this.transform.position;
        }

        protected override void onUpdate() {
            base.onUpdate();

            if (this.task != null) {
                if (!task.preform()) {
                    this.task = null;
                }
            }

            // Update stats.
            if(this.transform.position != this.lastPos) {
                this.unitStats.distanceWalked.increase(Vector3.Distance(this.transform.position, this.lastPos));
            }
            this.lastPos = this.transform.position;

            this.unitStats.timeAlive.increase(Time.deltaTime);
        }

        public override bool damage(int amount) {
            this.unitStats.damageTaken.increase(amount);
            return base.damage(amount);
        }

        public override void onDeathCallback() {
            base.onDeathCallback();

            // Hacky way to remove unit from the party.
            CameraMover cm = CameraMover.instance();
            if(cm.getControllingTeam() == this.getTeam()) {
                cm.party.remove(this);
            }

            GameObject.Destroy(this.gameObject);
        }

        public abstract EntityData getData();

        /// <summary>
        /// Sets the destination of the Unit's NavMeshAgent.  Point is not adjusted by any means, so add the units size in to the passed argument.
        /// Pass null for point to stop the agent.
        /// breakFlag sets how close the agent will move to the target.
        /// </summary>
        public void setDestination(Vector3? point, float breakFlag = 0) {
            if (point == null) {
                this.agent.SetDestination(this.transform.position);
            }
            else {
                this.agent.SetDestination((Vector3)point);
            }
            this.agent.stoppingDistance = breakFlag;
        }

        /// <summary>
        /// Sets the units task.  Pass null to cancel the current task.
        /// </summary>
        public void setTask(ITask task) {
            this.task = task;
        }

        /// <summary>
        /// Returns the Unit's current task, or null if it has none.
        /// </summary>
        public ITask getTask() {
            return this.task;
        }

        public override float getSizeRadius() {
            return 0.65f;
        }

        public override int getMaxHealth() {
            return this.getData().getHealth();
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

            if(obj.damage(damage)) {
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
