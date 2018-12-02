using codeshaper.data;
using codeshaper.entity.unit.stats;
using codeshaper.gui;
using codeshaper.registry;
using codeshaper.save;
using codeshaper.util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace codeshaper {

    // Note, set in Project Settings to run first.

    public class Main : MonoBehaviour {

        /// <summary> If true, debug mode is on. </summary>
        public static bool DEBUG = true;
        public static bool DEBUG_HEALTH = true;
        public static string SAVE_PATH = "saves/save1";

        private static Main singleton;

        public GameState gameState;

        private bool paused;

        public static Main instance() {
            return Main.singleton;
        }

        private void Awake() {
            if (Main.singleton == null) {
                Main.singleton = this;

                References.list = GameObject.FindObjectOfType<References>();

                // Preform bootstrap.
                Constants.bootstrap();
                Registry.registryBootstrap();
                GuiManager.guiBootstrap();
                Names.bootstrap();
            } else if (singleton != this) {
                // As every scene contains a Main object, destroy the new ones that are loaded.
                GameObject.Destroy(this.gameObject);
                return;
            }

            GameObject.DontDestroyOnLoad(gameObject);

            if(SceneManager.GetActiveScene().buildIndex == 0) {
                // Tn the title sceen, show the title screen GUI.
                GuiManager.openGui(GuiManager.titleScreen);
            } else {
                // Application started in a scene, this is in dev mode.
                // Load the save or create a new one.
                if(FileUtils.doesSaveExists()) {
                    this.gameState = new GameState(SAVE_PATH);
                } else {
                    this.startNewGame();
                }
            }
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

        /// <summary>
        /// Starts a new game and plays the cut scene.
        /// </summary>
        public void startNewGame() {
            this.gameState = new GameState();

            // Start cutscene.

            // After cutscene, load first level.
        }

        /// <summary>
        /// Loads the town scene.
        /// </summary>
        public void loadTownScene() {
            SceneManager.LoadScene("town/Town");
        }
    }
}
