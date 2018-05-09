using fNbt;
using src.buildings;
using src.buildings.harvestable;
using src.entity;
using src.registry;
using src.team;
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

        private void Awake() {
            Map.instance = this;

            this.entityHolder = this.findOrCreateHolder("ENTITY_HOLDER");
            this.buildingHolder = this.findOrCreateHolder("BUILDING_HOLDER");

            this.harvestableObjects = new List<HarvestableObject>();

            Team.initTeams();
        }

        private void Start() {
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
            NbtCompound rootTag = new NbtCompound();

            // Write the Harvestable Objects to nbt.
            NbtCompound tagHarvestables = new NbtCompound("harvestables");
            foreach(HarvestableObject obj in this.harvestableObjects) {
                tagHarvestables.Add(obj.writeToNbt(new NbtCompound()));
            }
            rootTag.Add(tagHarvestables);

            // Write all the Members of the team to NBT.
            NbtCompound tagTeams = new NbtCompound("teams");
            foreach(Team team in Team.ALL_TEAMS) {
                NbtCompound tagSpecificTeam = new NbtCompound(team.getName());
                foreach(SidedObjectBase obj in team.getMembers()) {
                    tagSpecificTeam.Add(obj.writeToNbt(new NbtCompound()));
                }
                tagTeams.Add(tagSpecificTeam);
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


            } else {
                // No save file found!
            }
        }

        /// <summary>
        /// Returns the name of the file that this scene should be saved to.
        /// </summary>
        private string getSaveFileName() {
            return "saves/save1/" + SceneManager.GetActiveScene().name + "nbt";
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
