using codeshaper.entity;
using codeshaper.map.chunk;
using codeshaper.map.generation.structure;
using codeshaper.registry;
using UnityEngine;

namespace codeshaper.map.generation {

    public class MapGeneratorPlains : MapGeneratorBase {

        public MapGeneratorPlains(Map map) : base(map) { }

        public override void generateChunk(Chunk chunk) {
            const float genZoneSize = 1.5f;

            System.Random rnd = new System.Random(this.seed);

            float x, z;
            int i;
            Vector3 vec = chunk.chunkPos.toWorldSpaceVector();

            if(chunk.chunkPos.x == 0 && chunk.chunkPos.z == 0) {
                // Spawn starting party.
                float f = Chunk.SIZEf / 2;
                this.map.spawnEntity(Registry.unitBuilder, this.spawnPoint + new Vector3(f + 1, 0, f + 1), this.getRndRot());
                this.map.spawnEntity(Registry.unitBuilder, this.spawnPoint + new Vector3(f + 0, 0, f + -2), this.getRndRot());
                this.map.spawnEntity(Registry.unitSoldier, this.spawnPoint + new Vector3(f + 1.5f, 0, f + -0.5f), this.getRndRot());
            }
            else {
                for (x = 0; x < Chunk.SIZE; x += genZoneSize) {
                    for (z = 0; z < Chunk.SIZE; z += genZoneSize) {
                        i = rnd.Next(0, 10000);
                        if (i < 400) {
                            this.spawnTree(vec.x + x, vec.z + z);
                        }
                    }
                }
            }
        }

        private void generateTerrain() {
            const int mapRadius = 100; // 250
            const int spawnZoneRadius = 4;
            const float genZoneSize = 1.5f;

            System.Random rnd = new System.Random(this.seed);

            int j = (mapRadius / 2) - (spawnZoneRadius * 2);
            this.spawnPoint = new Vector3(rnd.Next(-j, j), 0, rnd.Next(-j, j));

            float x, z;
            int i;
            for (x = -mapRadius; x < mapRadius; x += genZoneSize) {
                for (z = -mapRadius; z < mapRadius; z += genZoneSize) {
                    if((x < (-spawnZoneRadius + this.spawnPoint.x) || x > (spawnZoneRadius + this.spawnPoint.x)) || (z < (-spawnZoneRadius + this.spawnPoint.z) || z > (spawnZoneRadius + this.spawnPoint.z))) {
                        i = rnd.Next(0, 10000);
                        if(i == 0) {
                            new StructureFortress(this.map, new Vector3(x, 0, z), CameraMover.instance().getTeam(), seed).generate();
                        } else if (i < 30) {
                            this.map.spawnEntity(Registry.harvestableRock, new Vector3(x, 0, z), this.getRndRot());
                        } else if (i < 500) {
                            this.spawnTree(x, z);
                        }
                    }
                }
            }
        }

        private void spawnRock(float x, float z) {
            const float f1 = 2f;
            const float f2 = 3f;
            Vector3 scale = new Vector3(Random.Range(f1, f2), Random.Range(f1, f2), Random.Range(f1, f2));
            MapObject obj = this.map.spawnEntity(Registry.harvestableTree, new Vector3(x, 0, z), this.getRndRot(), scale);
        }

        /// <summary>
        /// Spawns a tree with a random size on the map.
        /// </summary>
        private void spawnTree(float x, float z) {
            float f = Random.Range(0.6f, 1.6f);
            Vector3 scale = new Vector3(f, f + (f * Random.Range(-0.2f, 0.2f)), f);
            MapObject obj = this.map.spawnEntity(Registry.harvestableTree, new Vector3(x, 0, z), this.getRndRot(), scale);
        }
    }
}
