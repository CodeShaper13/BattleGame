using src.data;
using System.Collections.Generic;
using UnityEngine;

namespace src.troop.task {

    public class TaskAttackNearby : TaskBase<UnitFighting> {

        protected const float RANGE = 20f;

        protected SidedObjectEntity target;
        protected float attackCooldown;

        public TaskAttackNearby(UnitFighting unit) : base(unit) {
            this.target = this.getClosestEnemy(RANGE);
        }

        public override bool preform() {
            this.attackCooldown -= Time.deltaTime;

            List<UnitBuilder> list = new List<UnitBuilder>();
            list.Add(null);

            if (this.findTarget()) {
                if(this.attackCooldown <= 0 && this.canReach(this.target, this.target.getSizeRadius() + this.unit.getSizeRadius())) {
                    this.target.damage(unit.getDamageDelt());
                    this.attackCooldown = Constants.TROOP_ATTACK_RATE;
                } else {
                    this.setDestination(this.target);
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if there is a target after calling this method.
        /// </summary>
        protected bool findTarget() {
            if (this.target == null || !this.canReach(this.target, RANGE)) {
                this.target = this.getClosestEnemy(RANGE);
            }

            return this.target != null;
        }
    }
}
