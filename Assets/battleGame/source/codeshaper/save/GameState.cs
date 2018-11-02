using codeshaper.util;
using fNbt;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

namespace codeshaper.save {

    public class GameState {

        private static string FILE_NAME = "/state.data";

        // References:
        public List<Level> allLevels = new List<Level>();

        // Game state:
        public bool foundYoke;
        public bool foundPlayingCard;
        public bool foundHammer;
        public bool foundReadingGlasses;
        public bool foundKey;
        
        public bool townObstacleRemoved;
        public bool seenCutscene;

        public Level testLevel;

        public Level tutorialLevel;
        public Level level1;
        public Level level2;
        public Level level3;
        public Level level4;
        public Level level5;

        /// <summary>
        /// Creates a new save.
        /// </summary>
        public GameState() {
            this.testLevel = new Level(this, "TestLevel");
            this.tutorialLevel = new Level(this, "TutorialLevel", this.level1);
            this.level1 = new Level(this, "Level1", this.level2);
            this.level2 = new Level(this, "Level2", this.level3);
            this.level3 = new Level(this, "Level3", this.level4);
            this.level4 = new Level(this, "Level4", this.level5);
            this.level5 = new Level(this, "Level5");

            if(!Util.doesSaveExists()) {
                // Save a "blank" game state so we know the game save exists.
                this.saveToFile();
            }
        }

        /// <summary>
        /// Loads a save from a file.
        /// </summary>
        public GameState(string path) : base() {
            string filePath = path + FILE_NAME;
            if (File.Exists(filePath)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(filePath);
                this.readFromNbt(file.RootTag);
            } else {
                // TODO error?
            }
        }

        /// <summary>
        /// Returns the current level that is being played, or null if a level scene is not loaded.
        /// </summary>
        public Level getCurrentLevel() {
            string name = SceneManager.GetActiveScene().name;
            foreach(Level level in this.allLevels) {
                if(level.getSceneName() == name) {
                    return level;
                }
            }
            return null;
        }

        private void readFromNbt(NbtCompound root) {
            NbtCompound unlockedItems = root.getCompound("unlockedItems");
            this.foundYoke = unlockedItems.getBool("foundYoke");
            this.foundPlayingCard = unlockedItems.getBool("foundPlayingCard");
            this.foundHammer = unlockedItems.getBool("foundHammer");
            this.foundReadingGlasses = unlockedItems.getBool("foundGlasses");
            this.foundKey = unlockedItems.getBool("foundKey");

            this.townObstacleRemoved = root.getBool("townObstacleRemoved");
            this.seenCutscene = root.getBool("seenCutscene");
        }

        /// <summary>
        /// Saves the game state to a file.
        /// </summary>
        public void saveToFile() {
            NbtCompound root = new NbtCompound("root");

            NbtCompound unlockedItems = new NbtCompound();
            unlockedItems.setTag("foundYoke", this.foundYoke);
            unlockedItems.setTag("foundPlayingCard", this.foundPlayingCard);
            unlockedItems.setTag("foundHammer", this.foundHammer);
            unlockedItems.setTag("foundGlasses", this.foundReadingGlasses);
            unlockedItems.setTag("foundKey", this.foundKey);

            root.setTag("townObstacleRemoved", this.townObstacleRemoved);
            root.setTag("seenCutscene", this.seenCutscene);

            NbtCompound levelsTag = new NbtCompound();
            foreach(Level level in this.allLevels) {
                levelsTag.Add(level.writeToNbt());
            }
            root.setTag("levels", levelsTag);

            NbtFile file = new NbtFile(root);
            file.SaveToFile(Main.SAVE_PATH + FILE_NAME, NbtCompression.None);
        }
    }
}
