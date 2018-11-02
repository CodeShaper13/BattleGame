using codeshaper.data;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity.unit.task {

    public class TaskDefendPoint : TaskAttackNearby {

        private Vector3 defendPoint;

        public TaskDefendPoint(UnitFighting unit) : base(unit) {
            this.defendPoint = this.unit.transform.position;
        }

        public override void drawDebug() {
            base.drawDebug();

            // Draw a line to mark the point and a line to conenct the unit to it's point.
            GLDebug.DrawLine(this.defendPoint, this.defendPoint + Vector3.up * 4, Color.black);
            GLDebug.DrawLine(this.unit.getPos(), this.defendPoint, Color.gray);
            foreach(Vector3 v in Direction.CARDINAL) {
                GLDebug.DrawLine(this.defendPoint, this.defendPoint + (v * Constants.AI_FIGHTING_DEFEND_RANGE), Color.black);
            }
        }

        protected override void preformAttack() {
            if (Util.isAlive(this.target) && this.withinArea()) {
                this.func();
            } else {
                // We have no target or it is out of range, find a new one.
                this.target = this.findTarget();
                if (this.target == null) {
                    // Move back to the defend point.
                    this.moveHelper.setDestination(this.defendPoint, 0.5f);
                }
            }
        }

        protected override SidedObjectEntity findTarget() {
            return this.findEntityOfType<SidedObjectEntity>(this.defendPoint, Constants.AI_FIGHTING_DEFEND_RANGE);
        }

        private bool withinArea() {
            return Vector2.Distance(this.defendPoint, this.target.getPos()) < Constants.AI_FIGHTING_DEFEND_RANGE;
        }
    }
}
