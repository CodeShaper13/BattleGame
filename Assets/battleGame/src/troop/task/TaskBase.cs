using src.team;
using UnityEngine;

namespace src.troop.task {

    public abstract class TaskBase<T> : ITask where T : UnitBase<T> {

        /// <summary>
        /// The Unit runnign the task.
        /// </summary>
        protected readonly T unit;

        public TaskBase(T unit) {
            this.unit = unit;
        }

        /// <summary>
        /// Returns the closest enemy object to this unit, or null if there are none.
        /// </summary>
        protected SidedObjectEntity getClosestEnemy(float maxDistance) {
            return this.getClosestEnemy(maxDistance, this.unit.transform.position);
        }

        /// <summary>
        /// Returns the closest enemy object, or null if there are none.
        /// </summary>
        protected SidedObjectEntity getClosestEnemy(float maxDistance, Vector3 point) {
            SidedObjectEntity obj = null;
            float f = float.PositiveInfinity;
            foreach (Team t in Team.ALL_TEAMS) {
                if(t != this.unit.getTeam()) {
                    foreach(SidedObjectBase s in t.getMembers()) {
                        if(s is SidedObjectEntity) {
                            float dis = Vector3.Distance(this.unit.transform.position, s.transform.position);
                            if ((dis < f) && (dis < maxDistance)) {
                                obj = (SidedObjectEntity)s;
                                f = dis;
                            }
                        }
                    }
                }
            }
            return obj;
        }

        protected void setDestination(SidedObjectEntity target) {
            this.unit.setDestination(target.transform.position, target.getSizeRadius() + this.unit.getSizeRadius() + 0.5f);
        }

        protected bool canReach(MonoBehaviour target, float maxDistance) {
            maxDistance += (0.5f);
            if(Vector3.Distance(this.unit.transform.position, target.transform.position) <= maxDistance) {
                return true;
            } else {
                return false;
            }
        }

        public abstract bool preform();
    }
}
