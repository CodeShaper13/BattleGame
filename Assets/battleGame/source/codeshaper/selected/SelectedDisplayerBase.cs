using UnityEngine;

namespace codeshaper.selected {

    public class SelectedDisplayerBase : MonoBehaviour {

        private Canvas canvas;

        protected virtual void Awake() {
            this.canvas = this.GetComponent<Canvas>();

            this.setVisible(false);
        }

        public void setVisible(bool visible) {
            this.canvas.enabled = visible;
        }
    }
}
