using src.button;

namespace src.troop {

    public abstract class UnitFighting : UnitBase<UnitFighting> {

        protected override void onUpdate() {
            base.onUpdate();
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.idle.mask | ActionButton.attackNearby.mask | ActionButton.defend.mask;
        }
    }
}
