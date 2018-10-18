using codeshaper.data;
using UnityEngine;

namespace codeshaper.entity.unit.task.attack {


    public class MeleeAttack {

        protected UnitBase unit;

        private float lastAttack;

        public MeleeAttack(UnitBase unit) {
            this.unit = unit;
        }

        public virtual SidedObjectEntity attack(SidedObjectEntity target) {
            if(Time.time >= (this.lastAttack + Constants.TROOP_ATTACK_RATE) && this.inRangeToAttack(target)) {
                this.preformAttack(target);
                this.lastAttack = Time.time;
            }
            return target;
        }

        /// <summary>
        /// Returns true if the target is in range.
        /// </summary>
        public virtual bool inRangeToAttack(SidedObjectEntity target) {
            float maxDistance = this.unit.getSizeRadius() + target.getSizeRadius() + 0.5f;
            return Vector3.Distance(this.unit.getPos(), target.transform.position) <= maxDistance;
        }

        protected virtual void preformAttack(SidedObjectEntity target) {
            target = this.unit.damageTarget(target);
        }
    }
}
