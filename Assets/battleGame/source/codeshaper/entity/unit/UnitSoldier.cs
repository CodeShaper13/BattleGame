using codeshaper.data;

namespace codeshaper.entity.unit {

    public class UnitSoldier : UnitFighting {

        public override EntityData getData() {
            return Constants.ED_SOLDIER;
        }
    }
}
