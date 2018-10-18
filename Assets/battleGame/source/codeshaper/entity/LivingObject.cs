using codeshaper.util;
using fNbt;
using UnityEngine;

namespace codeshaper.entity {

    public abstract class LivingObject : MapObject {

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
            if (this.health.setHealth(this.getHealth() - amount)) {
                this.onDeathCallback();
                GameObject.Destroy(this.gameObject);
                return true;
            } else {
                return false;
            }
        }

        public void setHealthbarVisibility(bool visible) {
            this.health.setVisible(visible);
        }

        public bool isDead() {
            return this.getHealth() <= 0;
        }

        public virtual void onDeathCallback() {
            // Implemented optionally in sub classes.
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.setHealth(tag.getInt("health", this.getMaxHealth()));
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("health", this.getHealth());
        }

        public abstract int getMaxHealth();

        public abstract float getHealthBarHeight();
    }
}
