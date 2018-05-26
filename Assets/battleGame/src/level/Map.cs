using fNbt;
using src.buildings;
using src.buildings.harvestable;
using src.entity;
using src.entity.projectiles;
using src.registry;
using src.team;
using src.util;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace src.map {

    /// <summary>
    /// Represents the current map.
    /// </summary>
    public class Map : MonoBehaviour {

        private static Map instance;

        private Transform entityHolder;
        private Transform buildingHolder;

        // Instance variables:
        public List<HarvestableObject> harvestableObjects = new List<HarvestableObject>();
        private List<Projectile> projectiles = new List<Projectile>();

        private void Awake() {
            Map.instance = this;

            this.entityHolder = this.findOrCreateHolder("ENTITY_HOLDER");
            this.buildingHolder = this.findOrCreateHolder("BUILDING_HOLDER");

            this.harvestableObjects = new List<HarvestableObject>();

            Team.initTeams();

            // Make sure onConstruct is called on every 
        }

        private void Start() {
            // Debug test:
            //this.spawnEntity(EntityRegistry.unitBuilder, new Vector3(0, 1, 0), Quaternion.identity);
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
        /// Spawns an Object into the world, loading its state from nbt and returns it.
        /// </summary>
        public SidedObjectBase spawnEntity(RegisteredObject obj, NbtCompound tag) {
            SidedObjectBase entity = this.instantiateEntityPrefab(obj.getPrefab());
            entity.readFromNbt(tag);
            return entity;
        }

        /// <summary>
        /// Spawns a new Object into the world.
        /// </summary>
        public SidedObjectBase spawnEntity(RegisteredObject obj, Vector3 position, Quaternion rotation) {
            SidedObjectBase entity = this.instantiateEntityPrefab(obj.getPrefab());
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            return entity;
        }

        /// <summary>
        /// Writes the Map to NBT.
        /// </summary>
        public NbtCompound writeToNbt() {
            NbtCompound rootTag = new NbtCompound("map");

            rootTag.Add(this.writeList<HarvestableObject>("harvestables", this.harvestableObjects));
            rootTag.Add(this.writeList<Projectile>("projectiles", this.projectiles));

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
                    NbtCompound tagSpecificTeam = tagTeams.getCompound(team.getName());
                    foreach(NbtCompound obj in tagSpecificTeam) {
                        int id = obj.getInt("id");
                        RegisteredObject registeredObject = Registry.getObjectfromRegistry(id);
                        this.spawnEntity(registeredObject, obj);
                    }
                }
            }
            else {
                // No save file found, this is the first time this level
                // is loaded for this save.
            }
        }

        private NbtList writeList<T>(string tagName, List<T> list) where T : MapObject {
            NbtList tagHarvestables = new NbtList(tagName);
            foreach (MapObject obj in list) {
                NbtCompound t = new NbtCompound();
                obj.writeToNbt(t);
                tagHarvestables.Add(t);
            }

            return tagHarvestables;
        }

        /// <summary>
        /// Returns the name of the file that this scene should be saved to.
        /// </summary>
        private string getSaveFileName() {
            return "saves/save1/" + SceneManager.GetActiveScene().name + ".nbt";
        }

        /// <summary>
        /// Instantiates a Prefab.
        /// </summary>
        private SidedObjectBase instantiateEntityPrefab(GameObject prefab, bool placeInWrapper = true) {
            GameObject obj = GameObject.Instantiate(prefab);
            SidedObjectBase entity = obj.GetComponent<SidedObjectBase>();

            if (placeInWrapper) {
                if(entity is BuildingBase) {
                    obj.transform.parent = this.buildingHolder;
                } else {
                    obj.transform.parent = this.entityHolder;
                }
            }
            entity.onConstruct();
            // Team stuff is set in the objects script.
            return entity;
        }

        /// <summary>
        /// Searches for a "holder" gameobject and if it can't be found,
        /// creates a new one to return.
        /// </summary>
        private Transform findOrCreateHolder(string name) {
            GameObject g = GameObject.Find("");
            if (g != null) {
                return g.transform;
            }
            else {
                return new GameObject(name).transform;
            }
        }
    }
}
