using UnityEngine;
using UnityEngine.UI;

namespace codeshaper.entity {

    /// <summary>
    /// Manages the health level and automatically updates the health bar ui effect.
    /// This script is NOT responsible for destroying GameObjects.
    /// </summary>
    public class Health : MonoBehaviour {

        public int maxHealth = -1;

        [SerializeField] // Serialized for debug reasons so we can see it in the inspector.
        private int health;
        [SerializeField]
        private RectTransform rect;
        private float originalX;
        private Image img;
        public Canvas canvas;

        public static Health instantiateHealthbar(GameObject holderObj, LivingObject living) {
            Vector3 pos = holderObj.transform.position + new Vector3(0, living.getHealthBarHeight(), 0);
            GameObject obj = GameObject.Instantiate(References.list.healthBarEffect, pos, Quaternion.identity);
            Health health = obj.GetComponent<Health>();
            health.transform.SetParent(holderObj.transform);
            health.gameObject.name = "HealthBarCanvas";
            health.setMaxHealth(living.getMaxHealth());
            return health;
        }

        private void Awake() {
            this.originalX = this.rect.sizeDelta.x;
            this.img = this.rect.GetComponent<Image>();
            //this.canvas = this.GetComponentInChildren<Canvas>();
        }

        private void Start() {
            this.setHealth(this.maxHealth);
        }

        /// <summary>
        /// Sets the maximum amount of health that this object can have.
        /// </summary>
        public void setMaxHealth(int amount) {
            this.maxHealth = amount;
            this.setHealth(this.maxHealth);
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

            // Update color.
            Color c;
            if(this.health < (this.maxHealth / 4)) {
                c = Color.red;
            } else if(this.health < (this.maxHealth / 2)) {
                c = new Color(1f, 0.4f, 0);
            } else {
                c = Color.green;
            }
            this.img.color = c;

            //if(this.health < this.maxHealth) {
            //    this.setVisible(true);
            //} else {
            //    this.setVisible(false);
            //}

            return (this.health <= 0);
        }

        public void setVisible(bool visible) {
            this.canvas.enabled = visible;
        }
    }
}
