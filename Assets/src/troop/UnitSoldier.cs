namespace src.troop {

    public class UnitSoldier : UnitFighting {

        public override int getMaxHealth() {
            return Constants.HEALTH_SOLDIER;
        }

        public override string getUnitName() {
            return "Warrior";
        }
    }
}
