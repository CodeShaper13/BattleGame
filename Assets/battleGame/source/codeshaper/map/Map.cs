using fNbt;
using codeshaper.buildings;
using codeshaper.buildings.harvestable;
using codeshaper.entity;
using codeshaper.registry;
using codeshaper.team;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using codeshaper.entity.projectiles;
using codeshaper.map.generation;
using codeshaper.nbt;
using codeshaper.map.chunk;
using codeshaper.map.chunkLoader;
using codeshaper.util;

namespace codeshaper.map {

    /// <summary>
    /// Represents the current map.
    /// </summary>
    public class Map : MonoBehaviour, INbtSerializable {

        private static Map singleton;

        public Transform holderChunk;
        private Transform holderLivingObject;
        private Transform holderProjectile;
        private Transform holderUnknown;

        public Dictionary<ChunkPos, Chunk> loadedChunks;

        private DayNightCycle dayNightCycle;

        public int seed;
        [Header("The ID of the map generator.  (0 = No Generator)")]
        public int generatorId; // Used so you can set the generator in the inspector.
        private MapGeneratorBase mapGenerator;
        private MapSaver mapSaver;
        public List<MapObject> mapObjects;

        public static Map instance() {
            return Map.singleton;
        }

        private void Awake() {
            Map.singleton = this;

            this.loadedChunks = new Dictionary<ChunkPos, Chunk>();
            this.dayNightCycle = this.GetComponentInChildren<DayNightCycle>();

            this.holderLivingObject = this.findOrCreateHolder("ENTITY");
            this.holderProjectile = this.findOrCreateHolder("PROJECTILE");
            this.holderChunk = this.findOrCreateHolder("CHUNK");
            this.holderUnknown = this.findOrCreateHolder("UNKNOWN");

            this.mapObjects = new List<MapObject>();

            // Reset all the teams.
            foreach (Team team in Team.ALL_TEAMS) {
                team.prepare(this);
            }
            this.mapSaver = new MapSaver(this, SceneManager.GetActiveScene().name);
            this.mapGenerator = MapGeneratorBase.getGenerator(this.generatorId, this);
        }

        private void Start() {
            // Temp, this is only for debugging.
            if(!this.mapSaver.exists()) {
                //this.newMap();
            } else {
                this.mapSaver.loadMap();
            }
        }

        private void Update() {
            if (!Main.instance().isPaused()) {
                foreach(Chunk chunk in this.loadedChunks.Values) {
                    chunk.onUpdate();
                }

                for (int i = this.mapObjects.Count - 1; i >= 0; i--) {
                    this.mapObjects[i].onUpdate();
                }
            }

            //NavMeshSurface surface = this.getChunk(new ChunkPos(0, 0)).GetComponent<NavMeshSurface>();
            //NavMeshData data = surface.navMeshData;
            //NavMesh.AddNavMeshData(surface.navMeshData);
            //print(NavMesh.CalculateTriangulation().indices.Length);
            //NavMeshTriangulation tris;
        }

        private void LateUpdate() {
            if(Main.DEBUG) {
                foreach (Chunk chunk in this.loadedChunks.Values) {
                    chunk.drawDebug();
                }
                for (int i = this.mapObjects.Count - 1; i >= 0; i--) {
                    this.mapObjects[i].drawDebug();
                }
            }
        }

        private void OnDestroy() {
            Map.singleton = null;
        }

        /// <summary>
        /// Saves the entire map to a file.
        /// </summary>
        public void saveMap() {
            foreach(Chunk chunk in this.loadedChunks.Values) {
                this.mapSaver.writeChunkToDisk(chunk, false);
            }
            this.mapSaver.saveMap();
        }

        /// <summary>
        /// Loads a new chunk, reading it from disk if it exists, otherwise a new one it generated.  The Chunk is then returned.
        /// </summary>
        public Chunk loadChunk(Chunk chunk, NewChunkInstructions instructions) {
            chunk.initChunk(this, instructions);

            this.loadedChunks.Add(chunk.chunkPos, chunk);

            if (!this.mapSaver.readChunkFromDisk(chunk)) {
                // Generate chunk and compute lighting.
                this.mapGenerator.generateChunk(chunk);

                Chunk adjacentChunk;
                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {
                        for (int z = -1; z <= 1; z++) {
                            if (!(x == 0 && y == 0 && z == 0)) { // Not the middle chunk.
                                adjacentChunk = this.getChunk(new ChunkPos(chunk.chunkPos.x + x, chunk.chunkPos.z + z));
                                //if (adjacentChunk != null && !adjacentChunk.hasDoneGen2) {
                                //    this.tryPhase2OnChunk(adjacentChunk);
                                //}
                            }
                        }
                    }
                }

                // It's possible that this chunk itself needs phase 2.
                //this.tryPhase2OnChunk(chunk);
            }
            return chunk;
        }

        /// <summary>
        /// Unloads the passed Chunk and saves the Chunk to disk.
        /// </summary>
        public void unloadChunk(Chunk chunk) {
            this.mapSaver.writeChunkToDisk(chunk, true);
            this.loadedChunks.Remove(chunk.chunkPos);
        }

        /// <summary>
        /// Returns the Chunk at the passed position.  Null is returned if the Chunk is not loaded.
        /// </summary>
        public Chunk getChunk(ChunkPos pos) {
            Chunk chunk = null;
            this.loadedChunks.TryGetValue(pos, out chunk);
            return chunk;
        }

        /// <summary>
        /// Returns the Chunk at the passed position.  Null is returned if the Chunk is not loaded.
        /// </summary>
        public Chunk getChunk(Vector3 worldPos) {
            return this.getChunk(new ChunkPos(
                MathHelper.floor(worldPos.x / Chunk.SIZEf),
                MathHelper.floor(worldPos.z / Chunk.SIZEf)));
        }

        /// <summary>
        /// Returns the closest HarvestableObject on the map.
        /// </summary>
        public MapObject getClosestObject(Vector3 point, Predicate<MapObject> validPredicate) {
            MapObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach(Chunk c in this.loadedChunks.Values) {
                //if(c.chunkBounds.SqrDistance(point) > closestDistanceSqr) {
                //    continue;
                //}
                foreach (MapObject potentialTarget in c.mapObjects) {
                    Vector3 directionToTarget = potentialTarget.getPos() - point;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr) {
                        if (validPredicate != null && !validPredicate(potentialTarget)) {
                            continue;
                        }
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = potentialTarget;
                    }
                }
            }
            return bestTarget;
        }

        /// <summary>
        /// Destroys the passed object.
        /// </summary>
        public void destroyObject(MapObject mapObject) {
            if(mapObject is HarvestableObject || mapObject is BuildingBase) {
                this.getChunk(mapObject.getPos()).mapObjects.Remove(mapObject);
            }
            else {
                this.mapObjects.Remove(mapObject);
            }

            if(mapObject is LivingObject) {
                ((LivingObject)mapObject).onDeathCallback();
            }

            GameObject.Destroy(mapObject.gameObject);
        }

        /// <summary>
        /// Spawns a MapObject into the World, loading it's state from NBT, then returns it object.
        /// </summary>
        public MapObject spawnEntity(RegisteredObject obj, NbtCompound tag) {
            MapObject mapObject = GameObject.Instantiate(obj.getPrefab()).GetComponent<MapObject>();
            #if UNITY_EDITOR
                mapObject.name += mapObject.getPos().ToString();
            #endif
            mapObject.readFromNbt(tag);

            this.addToList(mapObject);
            this.placeInWrapper(mapObject);

            return mapObject;
        }

        /// <summary>
        /// Spawns a new MapObject into the world and preforms the initial construct initialization, then returns it.
        /// </summary>
        public MapObject spawnEntity(RegisteredObject obj, Vector3 position, Quaternion rotation, Vector3? scale) {
            GameObject prefab = obj.getPrefab();
            MapObject mapObject = GameObject.Instantiate(prefab).GetComponent<MapObject>();

            // Set the Objects Transform.
            mapObject.transform.position = position;
            mapObject.transform.rotation = rotation;
            if(scale == null) {
                mapObject.transform.localScale = prefab.transform.localScale;
            } else {
                mapObject.transform.localScale = (Vector3)scale;
            }
            #if UNITY_EDITOR
                mapObject.name += mapObject.getPos().ToString();
            #endif
            // Call onInitialConstruct because this is a new object.
            mapObject.onInitialConstruct();

            this.addToList(mapObject);
            this.placeInWrapper(mapObject);

            return mapObject;
        }

        /// <summary>
        /// Spawns a new MapObject into the world and preforms the initial construct initialization, then returns it.
        /// </summary>
        public MapObject spawnEntity(RegisteredObject obj, Vector3 position, Quaternion rotation) {
            return this.spawnEntity(obj, position, rotation, null);
        }

        public List<MapObject> findMapObjects(Predicate<MapObject> predicate = null) {
            if(predicate == null) {
                return this.mapObjects; //TODO should a copy be returned?
            } else {
                return this.mapObjects.FindAll(predicate);
            }
        }

        public MapObject findMapObjectFromGuid(Guid guid) {
            foreach (SidedObjectEntity s in this.mapObjects) {
                if(s.getGuid() == guid) {
                    return s;
                }
            }
            MapObject obj;
            foreach(Chunk chunk in this.loadedChunks.Values) {
                obj = chunk.findObjectFromGuid(guid);
                if(obj != null) {
                    return obj;
                }
            }
            return null;
        }

        public void writeToNbt(NbtCompound tag) {
            tag.setTag("generatorId", this.generatorId);
            tag.setTag("seed", this.seed);
            tag.setTag("cameraPosition", CameraMover.instance().transform.parent.position);

            // Write all the Teams to NBT.
            NbtCompound tagTeams = new NbtCompound("teams");
            foreach(Team team in Team.ALL_TEAMS) {
                team.writeToNbt(tagTeams);
            }
            tag.Add(tagTeams);

            this.dayNightCycle.writeToNbt(tag);
        }

        public void readFromNbt(NbtCompound tag) {
            this.generatorId = tag.getInt("generatorId");
            this.seed = tag.getInt("seed");
            CameraMover.instance().setPostion(tag.getVector3("cameraPosition"));

            NbtCompound tagTeams = tag.getCompound("teams");
            foreach (Team team in Team.ALL_TEAMS) {
                team.readFromNbt(tagTeams);
            }

            this.dayNightCycle.readFromNbt(tag);
        }

        /// <summary>
        /// Adds the passed MapObject to the correct list, depending on it's type.
        /// </summary>
        private void addToList(MapObject obj) {
            if (obj is IStaticEntity) {
                Chunk chunk = this.getChunk(obj.getPos());
                chunk.func02(obj);
            }
            else { // Projectile or SidedObjectEntity
                this.mapObjects.Add(obj);
            }
        }

        /// <summary>
        /// Places the passed object in a wrapper to keep the hierarchy organized.
        /// </summary>
        public void placeInWrapper(MapObject mapObject) {
            if(!(mapObject is IStaticEntity)) { // Buildings and harvestables are stored in chunks.
                if (mapObject is Projectile) {
                    mapObject.transform.SetParent(this.holderProjectile);
                }
                else if (mapObject is SidedObjectEntity) {
                    mapObject.transform.SetParent(this.holderLivingObject);
                }
                else {
                    mapObject.transform.SetParent(this.holderUnknown);
                }
            }
        }

        /// <summary>
        /// Searches for a "holder" GameObject and if it can't be found,
        /// creates a new one.  The Holder Object is then returned.
        /// </summary>
        private Transform findOrCreateHolder(string name) {
            name += "_HOLDER";
            GameObject g = GameObject.Find(name);
            if (g != null) {
                return g.transform;
            }
            else {
                return new GameObject(name).transform;
            }
        }
    }
}