using src.data;

namespace src.troop {

    public class UnitHeavy : UnitFighting {

        public override int getMaxHealth() {
            return Constants.HEALTH_HEAVY;
        }

        public override string getUnitName() {
            return "Heavy";
        }

        public override int getDamageDelt() {
            return Constants.DAMAGE_HEAVY;
        }
    }
}
