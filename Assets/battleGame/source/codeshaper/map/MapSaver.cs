using codeshaper.map.chunk;
using fNbt;
using System.IO;

namespace codeshaper.map {

    public class MapSaver {

        private const string EXTENSION = ".nbt";

        private bool dontWriteToDisk = false;

        private Map map;
        private string saveName;

        private string saveFolderName;
        private string chunkFolderName;

        public MapSaver(Map map, string saveName) {
            this.map = map;
            this.saveName = saveName;

            this.saveFolderName = "saves/" + saveName + "/";
            this.chunkFolderName = this.saveFolderName + "chunks/";

            this.makeDirIfMissing(this.saveFolderName);
            this.makeDirIfMissing(this.chunkFolderName);
        }

        /// <summary>
        /// Returns true if this save can be found on the disk.
        /// </summary>
        public bool exists() {
            return File.Exists(this.getSaveFileName());
        }

        public void loadMap() {
            if (File.Exists(this.getSaveFileName())) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(saveName);

                NbtCompound rootTag = file.RootTag;
                this.map.readFromNbt(rootTag);
            }
        }

        public void saveMap() {
            if(this.dontWriteToDisk) {
                return;
            }

            NbtCompound rootTag = new NbtCompound("map");

            this.map.writeToNbt(rootTag);

            // Save the file.
            NbtFile file = new NbtFile(rootTag);
            file.SaveToFile(this.getSaveFileName(), NbtCompression.None);
        }

        /// <summary>
        /// Tries to read the passed chunk from the disk, returning true if it was found.
        /// </summary>
        public bool readChunkFromDisk(Chunk chunk) {
            string saveFile = this.getChunkFileName(chunk.chunkPos);

            if (File.Exists(saveFile)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(saveFile);
                chunk.readFromNbt(file.RootTag);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes the passed chunk to disk.
        /// </summary>
        public void writeChunkToDisk(Chunk chunk, bool deleteEntities) {
            if (this.dontWriteToDisk) {
                return;
            }

            NbtCompound tag = new NbtCompound("chunk");
            chunk.writeToNbt(tag, deleteEntities);

            NbtFile file = new NbtFile(tag);
            file.SaveToFile(this.getChunkFileName(chunk.chunkPos), NbtCompression.None);
        }

        /// <summary>
        /// Returns the name of the file that this map should be saved to.
        /// </summary>
        private string getSaveFileName() {
            return this.saveFolderName + this.saveName + MapSaver.EXTENSION;
        }

        private string getChunkFileName(ChunkPos chunkPos) {
            return this.saveFolderName + "chunks/" + chunkPos.x + "," + chunkPos.z + MapSaver.EXTENSION;
        }

        private void makeDirIfMissing(string name) {
            if (!this.dontWriteToDisk && !Directory.Exists(name)) {
                Directory.CreateDirectory(name);
            }
        }
    }
}
