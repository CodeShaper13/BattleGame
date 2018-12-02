using System;
using UnityEngine;

namespace codeshaper.map.chunk {

    [Serializable]
    public struct ChunkPos {

        public int x;
        public int z;

        public ChunkPos(int x, int z) {
            this.x = x;
            this.z = z;
        }

        /// <summary>
        /// Converts the chunk pos into a world space Vector3.
        /// </summary>
        public Vector3 toWorldSpaceVector() {
            return new Vector3(this.x * Chunk.SIZE, 0, this.z * Chunk.SIZE);
        }

        public override string ToString() {
            return "(" + this.x + ", " + this.z + ")";
        }

        public override bool Equals(object obj) {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 47;
                hash = hash * 227 + x;
                hash = hash * 227 + z;
                return hash;
            }
        }
    }
}

