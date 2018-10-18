using codeshaper.data;

namespace codeshaper.entity.unit {

    public class UnitHeavy : UnitFighting {

        public override EntityData getData() {
            return Constants.ED_HEAVY;
        }
    }
}
