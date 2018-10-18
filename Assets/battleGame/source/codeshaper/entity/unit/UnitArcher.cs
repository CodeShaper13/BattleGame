using codeshaper.data;

namespace codeshaper.entity.unit {

    public class UnitArcher : UnitFighting {

        public override EntityData getData() {
            return Constants.ED_ARCHER;
        }
    }
}
