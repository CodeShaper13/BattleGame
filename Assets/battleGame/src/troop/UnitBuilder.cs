using src.buildings;
using src.button;
using src.data;

namespace src.troop {

    public class UnitBuilder : UnitBase<UnitBuilder> {

        public int resources;

        private BuildingBase building;

        /// <summary>
        /// Sets the held resources to 0 and returns what
        /// the builder is carrying.
        /// </summary>
        public int deposite() {
            int i = this.resources;
            this.resources = 0;
            return i;
        }

        public bool canCarryMore() {
            return this.resources < Constants.BUILDER_MAX_CARRY;
        }

        public override int getMaxHealth() {
            return Constants.HEALTH_BUILDER;
        }

        public override string getUnitName() {
            return "Builder";
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.builderBuild.mask | ActionButton.harvestResources.mask;
        }

        public override int getDamageDelt() {
            return Constants.DAMAGE_BUILDER;
        }

        public void setBuilding(BuildingBase building) {
            this.building = building;
            this.gameObject.SetActive(false);
        }
    }
}
