using codeshaper.data;

namespace codeshaper.entity.unit.special {

    public class UnitWarWagon : UnitBase<UnitWarWagon> {

        public override EntityBaseStats getData() {
            return Constants.ED_WAR_WAGON;
        }
    }
}
