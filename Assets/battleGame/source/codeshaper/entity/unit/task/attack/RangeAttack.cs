using codeshaper.data;
using UnityEngine;

namespace codeshaper.entity.unit.task.attack {

    public class RangeAttack : MeleeAttack {

        public RangeAttack(UnitBase unit) : base(unit) {

        }

        protected override void preformAttack(SidedObjectEntity target) {
            base.preformAttack(target);
        }

        public override bool inRangeToAttack(SidedObjectEntity target) {
            return Vector3.Distance(this.unit.getPos(), target.getPos()) <= Constants.ARCHER_SHOOT_RANGE;
        }
    }
}
