using fNbt;
using src.button;
using src.util;

namespace src.entity {

    /// <summary>
    /// Represents an object that belongs to a certain side and has health.
    /// </summary>
    public abstract class SidedObjectEntity : SidedObjectBase, ILiving {

        private Health health;

        protected override void onStart() {
            base.onStart();

            this.health = Health.instantiateHealthbar(this.gameObject, this);
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

        /// <summary>
        /// Returns the bitmask of what buttons to display.
        /// </summary>
        public virtual int getButtonMask() {
            return ActionButton.destroy.mask;
        }

        /// <summary>
        /// Overried to stop the unit from allowing hte action buttons to be pressed
        /// depending on the units state.
        /// </summary>
        public virtual bool enableActionButton() {
            return true;
        }

        public abstract float getSizeRadius();

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.setHealth(tag.getInt("health"));
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.Add(new NbtInt("health", this.getHealth()));

            return tag;
        }

        // From ILiving
        public abstract int getMaxHealth();
        public abstract float getHealthBarHeight();
    }
}
