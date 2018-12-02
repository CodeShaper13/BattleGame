using codeshaper.map.chunk;

namespace codeshaper.map.chunkLoader {

    public struct NewChunkInstructions {

        public ChunkPos chunkPos;
        public bool isReadOnly;

        public NewChunkInstructions(int x, int z, bool isReadOnly) {
            this.chunkPos = new ChunkPos(x, z);
            this.isReadOnly = isReadOnly;
        }

        public override bool Equals(object obj) {
            return this.chunkPos.Equals(obj);
        }

        public override int GetHashCode() {
            return this.chunkPos.GetHashCode();
        }
    }
}

