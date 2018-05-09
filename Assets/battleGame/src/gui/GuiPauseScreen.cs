using src.map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace src.gui {

    public class GuiPauseScreen : MonoBehaviour {

        [SerializeField]
        private Button exitToTownButton;
        [SerializeField]
        private Button quitGameButton;

        private void Awake() {
            this.exitToTownButton.gameObject.SetActive(false);
            this.quitGameButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Enables the pause screen.
        /// </summary>
        public void enabeScreen() {
            this.gameObject.SetActive(true);

            if(Map.getInstance() == null) {
                // Must be in the town.
                this.quitGameButton.gameObject.SetActive(true);
            } else {
                this.exitToTownButton.gameObject.SetActive(true);
            }
        }

        public void disableScreen() {
            this.gameObject.SetActive(false);

            this.exitToTownButton.gameObject.SetActive(false);
            this.quitGameButton.gameObject.SetActive(false);
        }

        public void callback_resume() {
            this.disableScreen();
        }

        public void callback_exitToTown() {
            SceneManager.LoadScene("town");
        }

        public void callback_quitGame() {
            Application.Quit();
        }
    }
}