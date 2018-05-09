using fNbt;
using src.data;
using src.entity.unit.statistics;
using src.entity.unit.task;
using src.util;
using UnityEngine;
using UnityEngine.AI;

namespace src.entity.unit {

    public abstract class UnitBase : SidedObjectEntity {

        [HideInInspector]
        private NavMeshAgent agent;
        private ITask task;

        private Stats unitStats;
        private Vector3 lastPos;

        protected override void onAwake() {
            base.onAwake();
            this.agent = this.GetComponent<NavMeshAgent>();
            this.lastPos = this.transform.position;
        }

        protected override void onUpdate() {
            base.onUpdate();

            if (this.task != null) {
                if (!task.preform()) {
                    this.task = null;
                }
            }

            // Update Distance Moved stat.
            if(this.transform.position != this.lastPos) {
                this.unitStats.distanceWalked.increase(Vector3.Distance(this.transform.position, this.lastPos));
            }
            this.lastPos = this.transform.position;
        }

        public override void onDeathCallback() {
            base.onDeathCallback();
            CameraMover.instance().party.remove(this);
            GameObject.Destroy(this.gameObject);
        }

        public abstract EntityData getData();

        /// <summary>
        /// Easier way to get the position vector of the unit.
        /// </summary>
        public Vector3 getPos() {
            return this.transform.position;
        }

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
        /// Returns the Unit's current task.
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

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("lastPos", this.lastPos);
            this.unitStats.writeToNBT(tag);

            return tag;
        }
    }

    public abstract class UnitBase<T> : UnitBase where T : SidedObjectEntity { }
}
