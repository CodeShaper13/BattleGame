using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity.unit.task {

    public class TaskDefendPoint : TaskAttackNearby {

        private Vector3 defendPoint;

        public TaskDefendPoint(UnitFighting unit) : base(unit) {
            this.defendPoint = this.unit.transform.position;

            this.target = this.findEntityOfType<SidedObjectEntity>(FIND_RANGE);
        }

        protected override void preformAttack() {
            if (Util.isAlive(this.target) && Vector2.Distance(this.defendPoint, this.target.transform.position) < FIND_RANGE) {
                // Attack if we can reach the target.
                if (this.meleeAttack.inRangeToAttack(this.target)) {
                    this.target = this.meleeAttack.attack(this.target);
                } else {
                    this.setDestination(this.target);
                }
            } else {
                // We have no target or it is out of range, find a new one.
                this.target = this.findEntityOfType<SidedObjectEntity>(this.defendPoint, FIND_RANGE);
                if (this.target != null) {
                    this.setDestination(this.target);
                } else {
                    this.unit.setDestination(this.defendPoint);
                }
            }
        }
    }
}
