using codeshaper.buildings;
using codeshaper.buildings.harvestable;
using codeshaper.data;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity.unit.task.builder {

    public class TaskHarvestNearby : TaskBase<UnitBuilder> {

        private HarvestableObject target;
        private float cooldown;
        private BuildingBase dropoffPoint;

        public TaskHarvestNearby(UnitBuilder unit) : base(unit) {
        }

        public override void drawDebug() {
            base.drawDebug();

            // Draw lines to the target and drop off point.
            bool flag = this.unit.canCarryMore();
            if(Util.isAlive(this.target)) {
                GLDebug.DrawLine(this.unit.getPos(), this.target.getPos(), flag ? Color.green : Color.red);
            }
            if(this.dropoffPoint != null) {
                GLDebug.DrawLine(this.unit.getPos(), this.dropoffPoint.getPos(), flag ? Color.red : Color.green);
            }
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
                        this.moveHelper.setDestination(this.target.transform.position, this.target.getSizeRadius() + this.unit.getSizeRadius());
                    }
                }

                // Harvest from the nearby object if it is in range.
                if (this.cooldown <= 0 && Vector3.Distance(this.unit.getPos(), this.target.transform.position) <= (this.unit.getSizeRadius() + this.target.getSizeRadius())) {
                    if(this.target.harvest(this.unit)) {
                        // Target was consumed.
                        this.target = null; 
                    }
                    this.cooldown = Constants.BUILDER_STRIKE_RATE;
                }
            } else {
                // Full, drop off resources.
                if(this.dropoffPoint == null) {
                    // find a drop off point.
                    this.dropoffPoint = this.findEntityOfType<BuildingStoreroom>(this.unit.getPos(), -1, false);

                    if (this.dropoffPoint == null) {
                        // No drop off point, stop executing, as we can't drop off what we have.
                        return false;
                    } else {
                       // this.setDestination
                    }
                }                
            }

            return true;
        }

        /// <summary>
        /// Finds a harvestable object that the builder can harvest and returns it.
        /// </summary>
        private HarvestableObject findTarget() {
            HarvestableObject obj = null;
            float f = float.PositiveInfinity;
            foreach (HarvestableObject h in this.unit.map.getAllHarvestableObjects()) {
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
