using codeshaper.data;

namespace codeshaper.entity.unit {

    public class UnitHeavy : UnitFighting {

        public override EntityBaseStats getData() {
            return Constants.ED_HEAVY;
        }
    }
}
