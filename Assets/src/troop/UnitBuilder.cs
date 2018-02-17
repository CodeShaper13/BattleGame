using src.button;

namespace src.troop {

    public class UnitBuilder : UnitBase {

        public int resources;

        /// <summary>
        /// Sets the held resources to 0 and returns what
        /// the builder is carrying.
        /// </summary>
        public int deposite() {
            int i = this.resources;
            this.resources = 0;
            return i;
        }

        public override int getMaxHealth() {
            return Constants.HEALTH_BUILDER;
        }

        public override string getUnitName() {
            return "Builder";
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.builderBuild.getMask();
        }
    }
}
