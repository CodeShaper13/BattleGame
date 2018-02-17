namespace src.troop {

    public class UnitHeavy : UnitFighting {

        public override int getMaxHealth() {
            return Constants.HEALTH_HEAVY;
        }

        public override string getUnitName() {
            return "Heavy";
        }
    }
}
