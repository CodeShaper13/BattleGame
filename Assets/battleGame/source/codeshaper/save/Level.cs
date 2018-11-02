using codeshaper.util;
using fNbt;
using UnityEngine.SceneManagement;

namespace codeshaper.save {

    public class Level {

        private bool unlocked;
        private string sceneName;
        private Level[] unlocks;

        public Level(GameState state, string sceneName, params Level[] unlocks) {
            state.allLevels.Add(this);

            this.sceneName = sceneName;
        }

        public void readFromNbt(NbtCompound tag) {
            this.unlocked = tag.getBool("unlocked");
        }

        public NbtCompound writeToNbt() {
            NbtCompound tag = new NbtCompound();
            tag.setTag("unlocked", this.unlocked);
            return tag;
        }

        /// <summary>
        /// Loads this level's scene.  Does not check if it is unlocked.
        /// </summary>
        public void loadLevelScene() {
            SceneManager.LoadScene(this.sceneName);
        }

        /// <summary>
        /// Unlocks this level.
        /// </summary>
        public void unlock() {
            this.unlocked = true;
        }

        public bool isLocked() {
            return !this.unlocked;
        }

        public string getSceneName() {
            return this.sceneName;
        }

        public void unlockNextLevels() {
            foreach(Level l in this.unlocks) {
                l.unlock();
            }
        }
    }
}
