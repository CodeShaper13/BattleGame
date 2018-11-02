using codeshaper.button;

namespace codeshaper.entity.unit {

    public abstract class UnitFighting : UnitBase<UnitFighting> {

        protected override void onUpdate() {
            base.onUpdate();
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.idle.getMask() | ActionButton.attackNearby.getMask() | ActionButton.defend.getMask();
        }
    }
}
