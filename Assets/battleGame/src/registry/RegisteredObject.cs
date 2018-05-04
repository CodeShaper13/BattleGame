using UnityEngine;

namespace src.registry {

    public class RegisteredObject {

        private GameObject prefab;
        private int id;

        public RegisteredObject(int id, GameObject prefab) {
            this.id = id;
            this.prefab = prefab;
        }

        public GameObject getPrefab() {
            return this.prefab;
        }

        public int getId() {
            return this.id;
        }
    }
}
