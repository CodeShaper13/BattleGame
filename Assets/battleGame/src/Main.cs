using src.entity.unit.stats;
using src.gui;
using src.registry;
using UnityEngine;

namespace src {

    // Note, set in Project Settings to run first.

    public class Main : MonoBehaviour {

        private static Main singleton;

        private bool paused;

        public static Main instance() {
            return Main.singleton;
        }

        private void Awake() {
            if (Main.singleton == null) {
                Main.singleton = this;

                References.list = GameObject.FindObjectOfType<References>();

                // Preform bootstrap
                Registry.registryBootstrap();
                GuiManager.guiBootstrap();
                Names.bootstrap();
            }
            else if (singleton != this) {
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
            GuiManager.openGui(GuiManager.paused);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void resumeGame() {
            GuiManager.closeCurrentGui();
            this.paused = false;
            Time.timeScale = 1;
        }
    }
}
