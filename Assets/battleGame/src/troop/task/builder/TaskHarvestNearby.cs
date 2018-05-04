using src.buildings;
using src.buildings.harvestable;
using UnityEngine;

namespace src.troop.task.builder {

    public class TaskHarvestNearby : TaskBase<UnitBuilder> {

        private HarvestableObject target;
        private float cooldown;
        private BuildingBase dropoffPoint;

        public TaskHarvestNearby(UnitBuilder unit) : base(unit) {
        }

        public override bool preform() {
            this.cooldown -= Time.deltaTime;

            if(this.unit.canCarryMore()) {
                // Find something to harvest.
                if (this.target == null) {
                    this.target = this.findTarget();

                    if (this.target == null) {
                        return false; // No target to be found, stop executing.
                    } else {
                        this.unit.setDestination(this.target.transform.position, this.target.getSizeRadius() + this.unit.getSizeRadius());
                    }
                }

                // Harvest from the nearby object if it is in range.
                if (this.cooldown <= 0 && Vector3.Distance(this.unit.getPos(), this.target.transform.position) <= (this.unit.getSizeRadius() + this.target.getSizeRadius())) {
                    if(this.target.harvest(this.unit)) {
                        // Target was consumed.
                        this.target = null; 
                    }
                    this.cooldown = 1f;
                }
            } else {
                // Full, drop off resources.
                
            }

            return true;
        }

        /// <summary>
        /// Finds a harvestable object that the builder can harvest and returns it.
        /// </summary>
        private HarvestableObject findTarget() {
            HarvestableObject obj = null;
            float f = float.PositiveInfinity;
            foreach (HarvestableObject h in this.unit.map.harvestableObjects) {
                float dis = Vector3.Distance(this.unit.transform.position, h.transform.position);
                if (dis < f) {
                    obj = h;
                    f = dis;
                }
            }
            return obj;
        }
    }
}
