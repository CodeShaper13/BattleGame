using codeshaper.entity.unit.task.attack;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity.unit.task {

    public class TaskAttackNearby : TaskBase<UnitFighting> {

        protected const float FIND_RANGE = 20f;

        protected SidedObjectEntity target;
        protected MeleeAttack meleeAttack;

        public TaskAttackNearby(UnitFighting unit) : base(unit) {
            this.meleeAttack = new MeleeAttack(this.unit);

            this.target = this.findTarget();
        }

        public override bool preform() {
            this.preformAttack();

            return true;
        }

        protected virtual void preformAttack() {
            if (Util.isAlive(this.target) && Vector3.Distance(this.unit.getPos(), this.target.getPos()) <= FIND_RANGE) {
                if(this.meleeAttack.inRangeToAttack(this.target)) {
                    this.target = this.meleeAttack.attack(this.target);
                    //this.unit.agent.isStopped = true;
                } else {
                    //this.unit.agent.isStopped = false;
                    this.setDestination(this.target);
                }
            } else {
                // Target is dead or out of range, find a new one.
                this.target = this.findTarget();
            }
        }

        /// <summary>
        /// Tries to find a target, returning the closest target if one can be found
        /// or null if there are non in range.
        /// </summary>
        protected SidedObjectEntity findTarget() {
            return this.findEntityOfType<SidedObjectEntity>(FIND_RANGE);
        }
    }
}
