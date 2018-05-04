using src.data;

namespace src.troop {

    public class UnitSoldier : UnitFighting {

        public override int getMaxHealth() {
            return Constants.HEALTH_SOLDIER;
        }

        public override string getUnitName() {
            return "Warrior";
        }

        public override int getDamageDelt() {
            return Constants.DAMAGE_SOLDIER;
        }
    }
}
