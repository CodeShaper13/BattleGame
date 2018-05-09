using src.data;

namespace src.entity.unit {

    public class UnitArcher : UnitFighting {

        public override EntityData getData() {
            return Constants.ED_ARCHER;
        }
    }
}
