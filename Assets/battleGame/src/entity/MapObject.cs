using fNbt;
using src.map;
using src.util;
using UnityEngine;

namespace src.entity {

    public class MapObject : MonoBehaviour {

        /// <summary>
        /// A reference to the Map object representing the state of this map.
        /// </summary>
        [HideInInspector]
        public Map map;

        private void Awake() {
            this.onAwake();
        }

        private void Start() {
            this.map = Map.getInstance();

            this.onStart();
        }

        private void Update() {
            this.onUpdate();
        }

        protected virtual void onAwake() { }

        protected virtual void onStart() {

        }

        public virtual void onConstruct() { }

        protected virtual void onUpdate() { }

        /// <summary>
        /// Reads the object from NBT and sets it's state.
        /// </summary>
        public virtual void readFromNbt(NbtCompound tag) {
            this.transform.position = tag.getVector3("position");
            this.transform.eulerAngles = tag.getVector3("eulerRotation");
        }

        /// <summary>
        /// Writes the object to NBT and returns the tag.
        /// </summary>
        public virtual void writeToNbt(NbtCompound tag) {
            tag.setTag("position", this.transform.position);
            tag.setTag("eulerRotation", this.transform.eulerAngles);
        }
    }
}
