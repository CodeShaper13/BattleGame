using UnityEngine;
using UnityEngine.UI;

namespace codeshaper.util {

    public class FadeText : MonoBehaviour {

        public Text text;

        public Color fadeTo = Color.clear;
        [Header("How long until the font starts to vanish.")]
        public float timer;

        private Color originalColor;

        public void Awake() {
            this.originalColor = this.text.color;
        }

        public void Update() {
            if (this.timer > 0) {
                this.timer -= Time.deltaTime;
            }
            if (this.timer <= 0) {
                this.text.color = Color.Lerp(this.text.color, this.fadeTo, 3 * Time.deltaTime);
            }
        }

        public void showAndStartFade(string s, float time) {
            this.text.color = this.originalColor;
            this.text.text = s;
            this.timer = time;
        }
    }
}
