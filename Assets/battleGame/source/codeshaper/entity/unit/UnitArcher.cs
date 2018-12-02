using codeshaper.data;
using codeshaper.entity.unit.task.attack;

namespace codeshaper.entity.unit {

    public class UnitArcher : UnitFighting {

        public override EntityBaseStats getData() {
            return Constants.ED_ARCHER;
        }

        public override int getButtonMask() {
            return base.getButtonMask();
        }

        public override AttackBase createAttackMethod() {
            return new AttackArrow(this);
        }
    }
}
