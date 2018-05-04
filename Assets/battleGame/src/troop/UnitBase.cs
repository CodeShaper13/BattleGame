using src.troop.task;
using UnityEngine;
using UnityEngine.AI;

namespace src.troop {

    public abstract class UnitBase : SidedObjectEntity {

        [HideInInspector]
        private NavMeshAgent agent;
        private ITask task;

        protected override void onAwake() {
            base.onAwake();
            this.agent = this.GetComponent<NavMeshAgent>();
        }

        protected override void onUpdate() {
            base.onUpdate();

            if (this.task != null) {
                if (!task.preform()) {
                    this.task = null;
                }
            }
        }

        public override void onDeathCallback() {
            base.onDeathCallback();
            CameraMover.singleton.party.remove(this);
            GameObject.Destroy(this.gameObject);
        }

        public abstract int getDamageDelt();

        public abstract string getUnitName();

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
    }

    public abstract class UnitBase<T> : UnitBase where T : SidedObjectEntity { }
}
