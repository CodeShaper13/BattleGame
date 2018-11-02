using codeshaper.buildings;

namespace codeshaper.entity.unit.task {

    public class TaskConstructBuilding : TaskRepair {

        public TaskConstructBuilding(UnitBuilder unit, BuildingBase newBuilding) : base(unit, newBuilding, true) {
            this.building.setHealth(1);
            this.building.setConstructing();

            this.unit.unitStats.buildingsBuilt.increase();
        }

        public override bool cancelable() {
            return false;
        }

        protected override bool shouldContinue() {
            return this.building.isConstructing();

        }
    }
}
