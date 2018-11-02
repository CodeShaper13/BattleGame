

using codeshaper.buildings;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity.unit.task {

    public class TaskRepair : TaskBase<UnitBuilder> {

        protected BuildingBase building;
        protected bool isConstructing;

        public TaskRepair(UnitBuilder unit, BuildingBase newBuilding) : this(unit, newBuilding, false) { }

        protected TaskRepair(UnitBuilder unit, BuildingBase newBuilding, bool isConstructing) : base(unit) {
            this.building = newBuilding;
            this.isConstructing = isConstructing;

            this.moveHelper.setDestination(this.building);
        }

        public override bool preform() {
            if(Util.isAlive(this.building)) {
                if (this.nextToBuilding(this.building)) {
                    this.moveHelper.stop();
                    this.building.increaseConstructed(true);
                }
                else {
                    this.moveHelper.setDestination(this.building);
                }
                return this.shouldContinue();
            } else {
                return false; // End task.
            }
        }

        public override void drawDebug() {
            if (Util.isAlive(this.building)) {
                GLDebug.DrawLine(this.unit.getPos(), this.building.getPos(), this.nextToBuilding(this.building) ? Color.green : Color.red);
            }
        }

        protected virtual bool shouldContinue() {
            if (this.building.getHealth() == this.building.getMaxHealth()) {
                this.unit.unitStats.repairsDone.increase();
                return false;
            }
            return true;
        }
    }
}
