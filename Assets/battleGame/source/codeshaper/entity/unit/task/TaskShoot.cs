using codeshaper.data;
using UnityEngine;

namespace codeshaper.entity.unit.task {

    public class TaskShoot : TaskBase<UnitFighting> {

        protected const float MAX_SEE_TARGET_RANGE = 30f;
        protected const float FIRE_RANGE = 15f;
        protected const float STOP_FOLLOW_RANGE = 10f;

        private bool isWalking;
        private SidedObjectEntity target;
        private float fireCooldown;

        public TaskShoot(UnitFighting unit) : base(unit) { }

        public override bool preform() {
            this.fireCooldown -= Time.deltaTime;

            if (this.findTarget()) {
                // There is a target.

                if(this.isWalking) {
                    // Stop them from walking if the unit is close enough to start shooting.
                    if(this.canReach(this.target, STOP_FOLLOW_RANGE)) {
                        this.unit.setDestination(null);
                        this.isWalking = false;
                    }
                }

                if (this.isInFireRange()) {
                    if (this.fireCooldown <= 0) {
                        this.shoot();
                        this.fireCooldown = Constants.TROOP_FIRE_RATE;
                    }
                } else {
                    // Not in fire range, walk to target.
                    this.setDestination(this.target);
                    this.isWalking = true;
                }                
            }

            return true;
        }

        private bool isInFireRange() {
            return this.getDistance(this.target) < FIRE_RANGE;
        }

        private void shoot() {
            //TODO
        }

        /// <summary>
        /// Returns true if there is a target after calling this method.
        /// </summary>
        protected bool findTarget() {
            if (this.target == null || !this.canReach(this.target, MAX_SEE_TARGET_RANGE)) {
                this.target = this.findEntityOfType<SidedObjectEntity>(MAX_SEE_TARGET_RANGE);
            }

            return this.target != null;
        }
    }
}
