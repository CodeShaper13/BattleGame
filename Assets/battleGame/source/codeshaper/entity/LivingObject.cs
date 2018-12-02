using codeshaper.buildings.harvestable;
using codeshaper.nbt;
using fNbt;
using UnityEngine;

namespace codeshaper.entity {

    public abstract class LivingObject : MapObject {

        [SerializeField]
        [Header("Don't edit!  This is only shown for debuging!")]
        private int health;
        /// <summary> Null if this object doesn't have a health bar (it's on an enemy team). </summary>
        private ProgressBar healthBar;
        private bool shouldShowHealth;

        protected override void onAwake() {
            base.onAwake();
        }

        protected override void onStart() {
            base.onStart();

            if (this.health == 0) {
                // Nothing changed it.
                this.health = this.getMaxHealth();
            } // else Something set it, don't set health to max.

            this.shouldShowHealth = Main.DEBUG_HEALTH || this is HarvestableObject || (this is SidedObjectEntity && ((SidedObjectEntity)this).getTeam() == CameraMover.instance().getTeam());
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

            if(this.shouldShowHealth && this.shouldHealthbarBeShown()) {
                if(this.healthBar == null) {
                    this.healthBar = ProgressBar.instantiateHealthbar(this.gameObject, this.getHealthBarHeight(), this.getMaxHealth());
                }
                this.healthBar.updateProgressBar(amount);
                this.healthBar.setVisible(this.shouldHealthbarBeShown());
            }
        }

        /// <summary>
        /// Damages the Entity, returning true if the Entity was killed, false if it is still alive.
        /// </summary>
        public virtual bool damage(MapObject dealer, int amount) {
            this.setHealth(this.health - amount);
            if(this.health <= 0) {
                this.map.destroyObject(this);
                return true;
            } else {
                return false;
            }
        }

        public virtual bool shouldHealthbarBeShown() {
            return this.getHealth() != this.getMaxHealth();
        }

        /// <summary>
        /// Returns true if the object is dead.
        /// </summary>
        public bool isDead() {
            return this.health <= 0;
        }

        public virtual void onDeathCallback() {
            // Implemented optionally in sub classes.
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.health = tag.getInt("health");
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

        /// <summary>
        /// Returns the radius of this object.
        /// </summary>
        public abstract float getSizeRadius();
    }
}
