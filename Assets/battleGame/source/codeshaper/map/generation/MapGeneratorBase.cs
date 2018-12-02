using codeshaper.map.chunk;
using UnityEngine;

namespace codeshaper.map.generation {

    public class MapGeneratorBase {

        protected Map map;
        protected int seed;
        protected Vector3 spawnPoint;

        public MapGeneratorBase(Map map) {
            this.map = map;
            this.seed = map.seed;
        }

        public virtual void generateChunk(Chunk chunk) { }

        /// <summary>
        /// Returns a random rotation on the y axis.  The seed does NOT influence this.
        /// </summary>
        protected Quaternion getRndRot() {
            return Quaternion.Euler(0, Random.Range(0, 359), 0);
        }

        /// <summary>
        /// Returns the spawn point of the map.
        /// </summary>
        public Vector3 getSpawnPoint() {
            return this.spawnPoint;
        }

        /// <summary>
        /// Returns a generator from the passed ID.  An exception is thrown if the
        /// id is not valid.
        /// </summary>
        public static MapGeneratorBase getGenerator(int id, Map map) {
            if(id == 0) {
                return null;
            } else if(id == 1) {
                return new MapGeneratorPlains(map);
            } else {
                throw new System.Exception("Invalid ID (" + id + ") for a map generator type!");
            }
        }
    }
}
