using src.map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace src.gui {

    public class GuiPauseScreen : GuiScreen {

        [SerializeField]
        private Button exitToTownButton;
        [SerializeField]
        private Button quitGameButton;

        public override void onGuiInit() {
            this.exitToTownButton.gameObject.SetActive(false);
            this.quitGameButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Enables the pause screen.
        /// </summary>
        public override void onGuiOpen() {
            if(Map.getInstance() == null) {
                // Must be in the town.
                this.quitGameButton.gameObject.SetActive(true);
            } else {
                this.exitToTownButton.gameObject.SetActive(true);
            }
        }

        public override void onGuiClose() {
            // Hide buttons, they weill be reactivated when the gui opens.
            this.exitToTownButton.gameObject.SetActive(false);
            this.quitGameButton.gameObject.SetActive(false);
        }

        public void callback_resume() {
            Main.instance().resumeGame();
        }

        public void callback_exitToTown() {
            Map.getInstance().writeToNbt();
            //SceneManager.LoadScene("town");
        }

        public void callback_quitGame() {
            Application.Quit();
        }
    }
}