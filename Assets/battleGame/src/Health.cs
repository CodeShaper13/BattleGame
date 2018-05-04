using UnityEngine;

namespace src {

    /// <summary>
    /// Manages the health level and automatically updates the health bar ui effect.
    /// This script is NOT responsible for destroying GameObjects.
    /// </summary>
    public class Health : MonoBehaviour {

        private int maxHealth = 10;

        [SerializeField]
        private int health;
        [SerializeField]
        private RectTransform rect;
        private float originalX;

        private void Awake() {
            this.originalX = this.rect.sizeDelta.x;
        }

        private void Start() {
            this.setHealth(this.maxHealth);
        }

        /// <summary>
        /// Sets the maximum amount of health that this object can have.
        /// </summary>
        public void setMaxHealth(int amount) {
            this.maxHealth = amount;
        }

        /// <summary>
        /// Returns the current health of this object.
        /// </summary>
        public int getHealth() {
            return this.health;
        }

        /// <summary>
        /// Pass -1 to set max health.  Returns true if object is "dead" (it's health is 0 or less).
        /// </summary>
        public bool setHealth(int amount) {
            if(amount == -1) {
                amount = this.maxHealth;
            }

            this.health = Mathf.Clamp(amount, 0, this.maxHealth);
            float f = this.originalX / this.maxHealth;
            this.rect.sizeDelta = new Vector2(this.health * f, this.rect.sizeDelta.y);
            return this.health == 0;
        }
    }
}
