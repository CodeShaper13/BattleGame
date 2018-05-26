using src.buildings;
using src.team;
using UnityEngine;

namespace src.entity.unit.task {

    public abstract class TaskBase<T> : ITask where T : UnitBase<T> {

        /// <summary>
        /// The Unit running the task.
        /// </summary>
        protected readonly T unit;

        public TaskBase(T unit) {
            this.unit = unit;
        }

        /// <summary>
        /// Returns the closest enemy object to this unit, or null if there are none.
        /// </summary>
        protected T findEntityOfType<T>(float maxDistance) where T : SidedObjectEntity {
            return this.findEntityOfType<T>(this.unit.getPos(), maxDistance);
        }

        /// <summary>
        /// Returns the closest enemy object, or null if there are none.
        /// </summary>
        protected T findEntityOfType<T>(Vector3 point, float maxDistance = -1, bool findEnemies = true) where T : SidedObjectEntity {
            SidedObjectBase obj = null;
            float f = float.PositiveInfinity;
            Team thisTeam = this.unit.getTeam();
            foreach (Team t in Team.ALL_TEAMS) {
                if (findEnemies ? t != thisTeam : t == thisTeam) {
                    foreach (SidedObjectBase s in t.getMembers()) {
                        if (s is T) {
                            float dis = Vector3.Distance(this.unit.transform.position, s.transform.position);
                            if ((dis < f) && (maxDistance == -1 || dis < maxDistance)) {
                                obj = s;
                                f = dis;
                            }
                        }
                    }
                }
            }
            return (T)obj;
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
