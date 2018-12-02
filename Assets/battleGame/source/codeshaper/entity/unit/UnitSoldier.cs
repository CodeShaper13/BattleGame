using codeshaper.data;

namespace codeshaper.entity.unit {

    public class UnitSoldier : UnitFighting {

        public override EntityBaseStats getData() {
            return Constants.ED_SOLDIER;
        }
    }
}
