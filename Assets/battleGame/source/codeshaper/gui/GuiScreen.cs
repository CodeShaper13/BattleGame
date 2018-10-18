using UnityEngine;
using UnityEngine.UI;

namespace codeshaper.gui {

    public abstract class GuiScreen : MonoBehaviour {

        [SerializeField]
        protected Button closeButton;

        private void Awake() {
            if(this.closeButton != null) {
                this.closeButton.onClick.AddListener(GuiManager.closeCurrentGui);
            }

            this.onGuiInit();
        }

        private void OnEnable() {
            this.onGuiOpen();
        }

        private void OnDisable() {
            this.onGuiClose();
        }

        public virtual void onGuiInit() {

        }

        public virtual void onGuiOpen() {

        }

        public virtual void onGuiClose() {

        }
    }
}
