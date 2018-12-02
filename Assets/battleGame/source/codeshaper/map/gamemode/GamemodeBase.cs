using UnityEngine;
using UnityEngine.UI;

namespace codeshaper.map.gamemode {

    public abstract class GameModeBase : MonoBehaviour {

        [SerializeField]
        private Text text;

        /// <summary>
        /// The numver of seconds that are aloud for this level.  If time runs
        /// out, you lose.  Set to 0 to disable the timer.
        /// </summary>
        public float timeForMission;
        public bool countUp;
        private float timer;

        private void Start() {
            if(this.timeForMission != 0 && !this.countUp) {
                this.timer = this.timeForMission;
            }

            this.initializeGameMode();
        }

        private void Update() {
            if(this.timeForMission != 0) {
                if(this.countUp) {
                    this.timer += Time.deltaTime;
                    if(this.timer > timeForMission) {
                        this.triggerWin();
                    }
                } else {
                    this.timer -= Time.deltaTime;
                    if(this.timer <= 0f) {
                        this.triggerLoss();
                    }
                }
            }

            if(!Main.instance().isPaused()) {
                this.updateGameMode();
            }
        }

        protected virtual void initializeGameMode() { }

        protected virtual void updateGameMode() { }

        /// <summary>
        /// Call to trigger a win of the game mode.
        /// </summary>
        protected void triggerWin() {
            //TODO
        }

        /// <summary>
        /// Call to triger a loss of the game mode.
        /// </summary>
        protected void triggerLoss() {
            //TODO
        }

        /// <summary>
        /// Sets the text at the top of the screen.
        /// </summary>
        protected void setText(string text) {
            this.text.text = text;
        }

        protected abstract string getGameModeName();

        protected abstract string getDescription();
    }
}
