using UnityEngine;

namespace src {

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

        public void setMaxHealth(int amount) {
            this.maxHealth = amount;
        }

        public int getHealth() {
            return this.health;
        }

        /// <summary>
        /// Pass -1 for max health.  Returns true if the health is 0.
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
