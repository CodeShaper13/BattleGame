using src.data;

namespace src.troop {

    public class UnitArcher : UnitFighting {

        public override int getMaxHealth() {
            return Constants.HEALTH_ARCHER;
        }

        public override string getUnitName() {
            return "Archer";
        }

        public override int getDamageDelt() {
            return Constants.DAMAGE_ARCHER;
        }
    }
}
