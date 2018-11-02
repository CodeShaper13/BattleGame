using codeshaper.buildings.harvestable;
using codeshaper.util;
using fNbt;
using UnityEngine;

namespace codeshaper.entity {

    public abstract class LivingObject : MapObject {

        private int health;
        /// <summary> Null if this object doesn't have a health bar (it's on an enemy team). </summary>
        private ProgressBar healthBar;

        protected override void onAwake() {
            base.onAwake();

            this.setHealth(-1);
        }

        protected override void onStart() {
            base.onStart();

            if (Main.DEBUG || this is HarvestableObject || (this is SidedObjectEntity && ((SidedObjectEntity)this).getTeam() == CameraMover.instance().getTeam())) {
                this.healthBar = ProgressBar.instantiateHealthbar(this.gameObject, this.getHealthBarHeight(), this.getMaxHealth());
            }

            // Update the bar.
            this.setHealth(this.getHealth());
        }

        /// <summary>
        /// Returns the health of the entity.
        /// </summary>
        public int getHealth() {
            return this.health;
        }

        /// <summary>
        /// Should not be used to "damage" an object, as it does not record
        /// the damage to the stat list.
        /// Pass -1 to set max health.
        /// </summary>
        public void setHealth(int amount) {
            int maxHealth = this.getMaxHealth();
            if (amount == -1) {
                amount = maxHealth;
            }
            this.health = Mathf.Clamp(amount, 0, maxHealth);
            if(this.healthBar != null) {
                this.healthBar.updateProgressBar(amount);
            }
        }

        /// <summary>
        /// Damages the Entity, returning true if the Entity was killed, false if it is still alive.
        /// </summary>
        public virtual bool damage(MapObject dealer, int amount) {
            this.setHealth(this.health - amount);
            if(this.health <= 0) {
                this.onDeathCallback();
                GameObject.Destroy(this.gameObject);
                return true;
            } else {
                return false;
            }
        }

        public void setHealthbarVisibility(bool visible) {
            if(this.healthBar != null) {
                this.healthBar.setVisible(visible);
            }
        }

        public bool isDead() {
            return this.health <= 0;
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

            tag.setTag("health", this.health);
        }

        /// <summary>
        /// Returns the maximum amount of health this living object can have.
        /// </summary>
        public abstract int getMaxHealth();

        public abstract float getHealthBarHeight();
    }
}
