using src.data;
using UnityEngine;

namespace src.entity.unit.task {

    public class TaskDefendPoint : TaskAttackNearby {

        private Vector3 defendPoint;

        public TaskDefendPoint(UnitFighting unit) : base(unit) {
            this.defendPoint = this.unit.transform.position;

            this.target = this.findEntityOfType<SidedObjectEntity>(RANGE);
        }

        public override bool preform() {
            this.attackCooldown -= Time.deltaTime;

            if (this.target != null && Vector2.Distance(this.defendPoint, this.target.transform.position) < RANGE) {
                // Attack if we can reach the target.
                if (this.attackCooldown <= 0 && this.canReach(this.target, this.target.getSizeRadius() + this.unit.getSizeRadius())) {
                    this.unit.damageTarget(this.target);
                    this.attackCooldown = Constants.TROOP_ATTACK_RATE;
                }
                else {
                    this.setDestination(this.target);
                }
            } else {
                // We have no target or it is out of range, find a new one.
                this.target = this.findEntityOfType<SidedObjectEntity>(this.defendPoint, RANGE);
                if (this.target != null) {
                    this.setDestination(this.target);
                } else {
                    this.unit.setDestination(this.defendPoint);
                }
            }
            return true;
        }
    }
}
