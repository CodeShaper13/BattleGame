using src.button;

namespace src.troop {

    public abstract class UnitFighting : UnitBase {

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.holdGround.getMask();
        }
    }
}
