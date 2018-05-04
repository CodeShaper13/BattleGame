using UnityEngine.UI;

namespace src.util {

    /// <summary>
    /// Wrapper for both a button and it's text.
    /// </summary>
    public class ButtonWrapper {

        public Button button;
        private Text text;

        public ButtonWrapper(Button button) {
            this.button = button;
            this.text = this.button.GetComponentInChildren<Text>();
        }

        public void setText(string text) {
            this.text.text = text;
        }

        public void setActive(bool value) {
            this.button.gameObject.SetActive(value);
        }
    }
}
