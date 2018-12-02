using codeshaper.data;

namespace codeshaper.entity.unit.special {

    public class UnitCannon : UnitBase<UnitCannon> {

        public override EntityBaseStats getData() {
            return Constants.ED_CANNON;
        }
    }
}
