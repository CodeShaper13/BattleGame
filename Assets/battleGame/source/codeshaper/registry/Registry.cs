using codeshaper.entity;
using System;
using UnityEngine;

namespace codeshaper.registry {

    public class Registry {

        public static RegisteredObject unitBuilder;
        public static RegisteredObject unitSoldier;
        public static RegisteredObject unitArcher;
        public static RegisteredObject unitHeavy;

        public static RegisteredObject specialWarWagon;
        public static RegisteredObject specialCannon;

        public static RegisteredObject projectileArrow;

        public static RegisteredObject harvestableTree;
        public static RegisteredObject harvestableDeadTree;
        public static RegisteredObject harvestableCactus;
        public static RegisteredObject harvestableRock;
        public static RegisteredObject harvestableSkull;

        public static RegisteredObject buildingCamp;
        public static RegisteredObject buildingProducer;
        public static RegisteredObject buildingWorkshop;
        public static RegisteredObject buildingTrainingHouse;
        public static RegisteredObject buildingStoreroom;
        public static RegisteredObject buildingTower;
        public static RegisteredObject buldingWall;

        private const int REGISTRY_SIZE = 255;

        /// <summary> If true, the registry has been initialized. </summary>
        private static bool ranBootstrap;
        private static RegisteredObject[] objectRegistry;

        public Registry() {
            Registry.objectRegistry = new RegisteredObject[REGISTRY_SIZE];
        }

        /// <summary>
        /// Returns a RegisteredObject from the registry by ID, or null if nothing is registered with that id.
        /// </summary>
        public static RegisteredObject getObjectFromRegistry(int id) {
            if(id < 0 || id > REGISTRY_SIZE) {
                throw new Exception("Index out of registry range!");
            }
            return Registry.objectRegistry[id];
        }

        /// <summary>
        /// Returns the ID of the passed Entity, or -1 on error.
        /// </summary>
        public static int getIdFromObject(MapObject entity) {
            Type t = entity.GetType();
            RegisteredObject re;
            for (int i = 0; i < REGISTRY_SIZE; i++) {
                re = Registry.objectRegistry[i];
                if (re != null && t == re.getType()) {
                    return re.getId();
                }
            }
            return -1;
        }

        /// <summary>
        /// Initializes the registries if they haven't already been initialized.
        /// </summary>
        public static void registryBootstrap() {
            if (!Registry.ranBootstrap) {
                new Registry().registerAllObjects();
                Registry.ranBootstrap = true;
            }
        }

        private Registry registerAllObjects() {
            // Units are ids 0 - 31.
            Registry.unitBuilder = register(0, References.list.unitBuilder);
            Registry.unitSoldier = register(1, References.list.unitSoldier);
            Registry.unitArcher = register(2, References.list.unitArcher);
            Registry.unitHeavy = register(3, References.list.unitHeavy);

            // Special Unit ids are 32-63.
            Registry.specialWarWagon = register(32, References.list.specialWarWagon);
            Registry.specialCannon = register(33, References.list.specialCannon);

            // Harvestable ids are 64 - 95.
            Registry.harvestableTree = register(64, References.list.harvestableTreePrefab);
            Registry.harvestableDeadTree = register(65, References.list.harvestableDeadTreePrefab);
            Registry.harvestableCactus = register(66, References.list.harvestableCactusPrefab);
            Registry.harvestableRock = register(67, References.list.harvestableRockPrefab);
            Registry.harvestableSkull = register(68, References.list.harvestableSkullPrefab);

            // Projectile ids are 96-127.

            Registry.projectileArrow = register(96, References.list.projectileArrow);

            // Buildings are ids 128 - 255.
            Registry.buildingCamp = register(128, References.list.buildingCamp);
            Registry.buildingProducer = register(129, References.list.buildingProducer);
            Registry.buildingWorkshop = register(130, References.list.buildingWorkshop);
            Registry.buildingTrainingHouse = register(131, References.list.buildingTrainingHouse);
            Registry.buildingStoreroom = register(132, References.list.buildingStoreroom);
            Registry.buildingTower = register(133, References.list.buildingTower);
            Registry.buldingWall = register(134, References.list.buldingWall);

            return this;
        }

        private RegisteredObject register(int id, GameObject prefab) {
            RegisteredObject registerObject = new RegisteredObject(id, prefab);

            int i = registerObject.getId();
            if (Registry.objectRegistry[i] != null) {
                throw new Exception("Two objects were registered with the same ID!");
            }
            Registry.objectRegistry[i] = registerObject;
            return registerObject;
        }

        private RegisteredObject register(int id, string prefabName) {
            return this.register(id, Resources.Load<GameObject>(prefabName));
        }
    }
}
