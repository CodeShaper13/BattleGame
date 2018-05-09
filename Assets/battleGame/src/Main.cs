using src.gui;
using UnityEngine;

namespace src {

    public class Main : MonoBehaviour {

        private static Main singleton;

        private bool paused;

        [SerializeField]
        private GuiPauseScreen pauseScreen;


        public static Main instance() {
            return Main.singleton;
        }

        private void Awake() {
            if (Main.singleton == null) {
                Main.singleton = this;
            } else if (singleton != this) {
                GameObject.Destroy(this.gameObject);
                return;
            }

            GameObject.DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Returns true if the game is paused.
        /// </summary>
        public bool isPaused() {
            return this.paused;
        }

        /// <summary>
        /// Pauses the game and handles the blocking of input.
        /// </summary>
        public void pauseGame() {
            this.paused = true;
            Time.timeScale = 0;
            this.pauseScreen.enabeScreen();
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void resumeGame() {
            this.pauseScreen.disableScreen();
            this.paused = false;
            Time.timeScale = 1;
        }
    }
}
