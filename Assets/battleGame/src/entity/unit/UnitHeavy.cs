using src.data;

namespace src.entity.unit {

    public class UnitHeavy : UnitFighting {

        public override EntityData getData() {
            return Constants.ED_HEAVY;
        }
    }
}
