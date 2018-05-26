using src.entity;
using System;
using UnityEngine;

namespace src.registry {

    public class Registry {

        public static RegisteredObject unitBuilder;
        public static RegisteredObject unitSoldier;
        public static RegisteredObject unitArcher;
        public static RegisteredObject unitHeavy;

        public static RegisteredObject buildingCamp;
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
        public static RegisteredObject getObjectfromRegistry(int id) {
            if(id < 0 || id > REGISTRY_SIZE) {
                throw new Exception("Index out of range!");
            }
            return Registry.objectRegistry[id];
        }

        /// <summary>
        /// Returns the ID of the passed Entity, or -1 on error.
        /// </summary>
        public static int getIdFromObject(SidedObjectEntity entity) {
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
            // Units are ids 0 - 127.
            Registry.unitBuilder = register(0, References.list.unitBuilder);
            Registry.unitSoldier = register(1, References.list.unitSoldier);
            Registry.unitArcher = register(2, References.list.unitArcher);
            Registry.unitHeavy = register(3, References.list.unitHeavy);

            // Buildings are ids 128 - 255.
            Registry.buildingCamp = register(128, References.list.buildingCamp);
            Registry.buildingWorkshop = register(129, References.list.buildingWorkshop);
            Registry.buildingTrainingHouse = register(130, References.list.buildingTrainingHouse);
            Registry.buildingStoreroom = register(131, References.list.buildingStoreroom);
            Registry.buildingTower = register(132, References.list.buildingTower);
            Registry.buldingWall = register(133, References.list.buldingWall);

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
