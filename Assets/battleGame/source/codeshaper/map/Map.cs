using fNbt;
using codeshaper.buildings;
using codeshaper.buildings.harvestable;
using codeshaper.entity;
using codeshaper.registry;
using codeshaper.team;
using codeshaper.util;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using codeshaper.entity.projectiles;

namespace codeshaper.map {

    /// <summary>
    /// Represents the current map.
    /// </summary>
    public class Map : MonoBehaviour {

        private static Map instance;

        private Transform holderLivingObject;
        private Transform holderBuilding;
        private Transform holderProjectile;
        private Transform holderUnknown;

        private List<MapObject> mapObjects;

        // Instance variables:
        private List<HarvestableObject> harvestableObjects = new List<HarvestableObject>();

        private void Awake() {
            Map.instance = this;

            this.holderLivingObject = this.findOrCreateHolder("ENTITY_HOLDER");
            this.holderBuilding = this.findOrCreateHolder("BUILDING_HOLDER");
            this.holderProjectile = this.findOrCreateHolder("PROJECTILE_HOLDER");
            this.holderUnknown = this.findOrCreateHolder("UNKNOWN_HOLDER");

            this.mapObjects = new List<MapObject>();
            this.harvestableObjects = new List<HarvestableObject>();

            // Make sure the team objects are ready to be used.
            Team.resetTeams();

            // Make sure onConstruct is called on every 
        }

        private void Start() {
            // Debug test:
            //this.spawnEntity(EntityRegistry.unitBuilder, new Vector3(0, 1, 0), Quaternion.identity);
        }

        private void Update() {
            //foreach(Team t in Team.ALL_TEAMS) {
            //    print("Team: " + t.getName());
            //    foreach(SidedObjectEntity soe in t.getMembers()) {
            //        print("   " + soe.name);
            //    }
            //}
        }

        private void OnDestroy() {
            Map.instance = null;
        }

        public static Map getInstance() {
            return Map.instance;
        }

        public void addHarvestableObject(HarvestableObject obj) {
            this.harvestableObjects.Add(obj);
        }

        public void removeHarvestableObject(HarvestableObject obj) {
            this.harvestableObjects.Remove(obj);
        }

        /// <summary>
        /// Returns a list of all harvestable objects on the map.
        /// </summary>
        public List<HarvestableObject> getAllHarvestableObjects() {
            return this.harvestableObjects;
        }

        /// <summary>
        /// Spawns an Object into the world, loading it's state from nbt and returns it.
        /// </summary>
        public MapObject spawnEntity(RegisteredObject obj, NbtCompound tag) {
            MapObject mapObject = this.instantiateMapObject(obj.getPrefab());
            mapObject.readFromNbt(tag);
            return mapObject;
        }

        /// <summary>
        /// Spawns a new Object into the world.
        /// </summary>
        public MapObject spawnEntity(RegisteredObject obj, Vector3 position, Quaternion rotation) {
            MapObject mapObject = this.instantiateMapObject(obj.getPrefab());
            mapObject.transform.position = position;
            mapObject.transform.rotation = rotation;
            return mapObject;
        }

        public SidedObjectEntity getSidedObjectFromGuid(Guid guid) {
            foreach(Team t in Team.ALL_TEAMS) {
                foreach (SidedObjectEntity s in t.getMembers()) {
                    if(s.getGuid() == guid) {
                        return s;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Writes the Map to NBT and saves it to a file.
        /// </summary>
        public NbtCompound writeToNbt() {
            NbtCompound rootTag = new NbtCompound("map");

            this.writeList<HarvestableObject>(rootTag, "harvestables", this.harvestableObjects);

            // Write all the Members of the team to NBT.
            NbtCompound tagTeams = new NbtCompound("teams");
            foreach(Team team in Team.ALL_TEAMS) {
                tagTeams.Add(team.write());
            }
            rootTag.Add(tagTeams);

            // Save the file.
            NbtFile file = new NbtFile(rootTag);
            file.SaveToFile(this.getSaveFileName(), NbtCompression.None);
            return rootTag;
        }

        public void readFromNbt(NbtCompound tag) {
            string s = this.getSaveFileName();

            if (File.Exists(s)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(s);

                NbtCompound rootTag = file.RootTag;

                NbtCompound tagTeams = rootTag.getCompound("teams");
                foreach (Team team in Team.ALL_TEAMS) {
                    team.read(this, rootTag.getCompound(team.getName()));
                }
            }
            else {
                // No save file found, this is the first time this level
                // is loaded for this save.
            }
        }

        private void writeList<T>(NbtCompound rootTag, string tagName, List<T> list) where T : MapObject {
            if(list.Count == 0) {
                return;
            }
            NbtList tagList = new NbtList(tagName);
            foreach (MapObject obj in list) {
                NbtCompound t = new NbtCompound();
                obj.writeToNbt(t);
                tagList.Add(t);
            }
            rootTag.Add(tagList);
        }

        /// <summary>
        /// Returns the name of the file that this scene should be saved to.
        /// </summary>
        private string getSaveFileName() {
            return "saves/save1/" + SceneManager.GetActiveScene().name + ".nbt";
        }

        public MapObject instantiateMapObject(GameObject prefab, bool placeInWrapper = true) {
            GameObject obj = GameObject.Instantiate(prefab);
            MapObject mapObject = obj.GetComponent<MapObject>();

            if (placeInWrapper) {
                if (mapObject is BuildingBase) {
                    obj.transform.parent = this.holderBuilding;
                }
                else if(mapObject is Projectile) {
                    obj.transform.parent = this.holderProjectile;
                }
                else if(mapObject is SidedObjectEntity) {
                    obj.transform.parent = this.holderLivingObject;
                }
                else {
                    obj.transform.parent = this.holderUnknown;
                }
            }
            mapObject.onConstruct();
            // Team stuff is set in the objects script.
            return mapObject;
        }

        /// <summary>
        /// Searches for a "holder" gameobject and if it can't be found,
        /// creates a new one to return.
        /// </summary>
        private Transform findOrCreateHolder(string name) {
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
