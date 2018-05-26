using fNbt;
using src.button;
using src.registry;
using src.util;

namespace src.entity {

    /// <summary>
    /// Represents an object that belongs to a certain side and has health.
    /// </summary>
    public abstract class SidedObjectEntity : SidedObjectBase, ILiving {

        private Health health;

        protected override void onAwake() {
            base.onAwake();

            this.health = Health.instantiateHealthbar(this.gameObject, this);
        }

        /// <summary>
        /// Returns the health of the entity.
        /// </summary>
        public int getHealth() {
            return this.health.getHealth();
        }

        /// <summary>
        /// Should not be used to "damage" an object, as it does not record
        /// the damage to the stat list.
        /// </summary>
        public void setHealth(int amount) {
            this.health.setHealth(amount);
        }

        /// <summary>
        /// Damages the Entity, returning true if the Entity was killed, false if it is still alive.
        /// </summary>
        public virtual bool damage(int amount) {
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

            // Do not read id tag.
            this.setHealth(tag.getInt("health"));
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("id", Registry.getIdFromObject(this));
            tag.setTag("health", this.getHealth());
        }

        // From ILiving
        public abstract int getMaxHealth();
        public abstract float getHealthBarHeight();
    }
}
