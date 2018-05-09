using System;
using UnityEngine;

namespace src.registry {

    public abstract class Registry {

        public static BuildingRegistry buildingRegistry;
        public static EntityRegistry entityRegistry;

        private const int REGISTRY_SIZE = 64;

        /// <summary>
        /// If true, the registry has been initialized.
        /// </summary>
        private static bool ranBootstrap;

        private RegisteredObject[] objectRegistry;

        public Registry() {
            this.objectRegistry = new RegisteredObject[REGISTRY_SIZE];
        }

        protected RegisteredObject register(int id, GameObject prefab) {
            RegisteredObject registerObject = new RegisteredObject(id, prefab);

            int i = registerObject.getId();
            if(this.objectRegistry[i] != null) {
                throw new Exception("Two objects were registered with the same ID!");
            }
            this.objectRegistry[i] = registerObject;
            return registerObject;
        }

        protected RegisteredObject register(int id, string prefabName) {
            return this.register(id, Resources.Load<GameObject>(prefabName));
        }

        /// <summary>
        /// Returns a RegisteredObject from the registry by ID.
        /// </summary>
        public RegisteredObject getObjectfromRegistry(int id) {
            if(id < 0 || id > REGISTRY_SIZE) {
                throw new Exception("Index out of range!");
            }
            return this.objectRegistry[id];
        }

        protected abstract Registry initRegistry();

        /// <summary>
        /// Initializes the registries if they haven't already been initialized.
        /// </summary>
        public static void registryBootstrap() {
            if (!Registry.ranBootstrap) {
                Registry.entityRegistry = ((EntityRegistry) new EntityRegistry().initRegistry());
                Registry.buildingRegistry = ((BuildingRegistry) new BuildingRegistry().initRegistry());
                Registry.ranBootstrap = true;
            }
        }
    }
}
