using UnityEngine;

namespace codeshaper.entity.unit.task.attack {


    public class AttackMelee : AttackBase {

        public AttackMelee(UnitBase unit) : base(unit) { }

        /// <summary>
        /// Returns true if the target is in range.
        /// </summary>
        public override bool inRangeToAttack(SidedObjectEntity target) {
            float maxDistance = this.unit.getSizeRadius() + target.getSizeRadius() + 0.5f;
            return Vector3.Distance(this.unit.getPos(), target.transform.position) <= maxDistance;
        }

        protected override void preformAttack(SidedObjectEntity target) {
            this.unit.damageTarget(target);
        }
    }
}
