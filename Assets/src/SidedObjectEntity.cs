using src.button;

namespace src {

    public abstract class SidedObjectEntity : SidedObjectBase {

        private Health health;

        protected override void onAwake() {
            base.onAwake();

            this.health = this.GetComponentInChildren<Health>();
            this.health.setMaxHealth(this.getMaxHealth());
        }

        public int getHealth() {
            return this.health.getHealth();
        }

        public void setHealth(int amount) {
            this.health.setHealth(amount);
        }

        public bool damage(int amount) {
            if(this.health.setHealth(this.health.getHealth() - amount)) {
                this.team.leave(this);
                this.onDeathCallback();
                return true;
            } else {
                return false;
            }
        }

        public virtual void onDeathCallback() {

        }

        public virtual void onClick(CameraMover camera) {
            this.damage(2);
        }

        public virtual int getButtonMask() {
            return ActionButton.destroy.getMask();
        }

        public abstract int getMaxHealth();
    }
}
