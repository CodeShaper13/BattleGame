using src.entity;
using System;
using UnityEngine;

namespace src.registry {

    public class RegisteredObject {

        private readonly GameObject prefab;
        private readonly int id;
        private readonly Type type;

        public RegisteredObject(int id, GameObject prefab) {
            this.id = id;
            this.prefab = prefab;
            this.type = this.prefab.GetComponent<SidedObjectEntity>().GetType();
        }

        public GameObject getPrefab() {
            return this.prefab;
        }

        public int getId() {
            return this.id;
        }

        public Type getType() {
            return this.type;
        }
    }
}
