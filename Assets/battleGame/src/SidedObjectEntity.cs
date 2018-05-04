using fNbt;
using src.button;

namespace src {

    /// <summary>
    /// Represents an object that belongs to a certain side and has health.
    /// </summary>
    public abstract class SidedObjectEntity : SidedObjectBase {

        private Health health;

        protected override void onAwake() {
            base.onAwake();

            this.health = this.GetComponentInChildren<Health>();
            this.health.setMaxHealth(this.getMaxHealth());
        }

        /// <summary>
        /// Returns the health of the entity.
        /// </summary>
        public int getHealth() {
            return this.health.getHealth();
        }

        public void setHealth(int amount) {
            this.health.setHealth(amount);
        }

        public bool damage(int amount) {
            if(this.health.setHealth(this.health.getHealth() - amount)) {
                this.getTeam().leave(this);
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
            return ActionButton.destroy.mask;
        }

        public abstract int getMaxHealth();

        public abstract float getSizeRadius();

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.Add(new NbtInt("health", this.getHealth()));

            return tag;
        }
    }
}
