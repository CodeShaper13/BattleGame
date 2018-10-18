using codeshaper.team;
using UnityEngine;

namespace codeshaper.entity.unit.task {

    public abstract class TaskBase<T> : ITask where T : UnitBase {

        /// <summary>
        /// The Unit running the task.
        /// </summary>
        protected readonly T unit;

        public TaskBase(T unit) {
            this.unit = unit;
        }

        /// <summary>
        /// Returns the closest enemy object to this unit, or null if there are none in range.
        /// </summary>
        protected T findEntityOfType<T>(float maxDistance) where T : SidedObjectEntity {
            return this.findEntityOfType<T>(this.unit.getPos(), maxDistance);
        }

        /// <summary>
        /// Returns the closest enemy object, or null if there are none.
        /// </summary>
        protected T findEntityOfType<T>(Vector3 point, float maxDistance = -1, bool findEnemies = true) where T : SidedObjectEntity {
            SidedObjectEntity obj = null;
            float f = float.PositiveInfinity;
            Team thisTeam = this.unit.getTeam();
            foreach (Team team in Team.ALL_TEAMS) {
                if (findEnemies ? team != thisTeam : team == thisTeam) {
                    foreach (SidedObjectEntity s in team.getMembers()) {
                        if (s is T && !s.isDead()) {
                            float dis = Vector3.Distance(point, s.transform.position);
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

        /// <summary>
        /// Sets the destination that this unit should move to.
        /// </summary>
        protected void setDestination(SidedObjectEntity target) {
            this.unit.setDestination(target.transform.position, target.getSizeRadius() + this.unit.getSizeRadius() + 0.5f);
        }

        protected bool canReach(MapObject target, float maxDistance) {
            maxDistance += (0.5f);
            return Vector3.Distance(this.unit.getPos(), target.getPos()) <= maxDistance;
        }

        /// <summary>
        /// Returns the distance between this unit and the passed entity.
        /// </summary>
        protected float getDistance(SidedObjectEntity other) {
            return Vector3.Distance(this.unit.transform.position, other.transform.position);
        }

        public abstract bool preform();
    }
}
