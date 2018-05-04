using fNbt;
using src.map;
using UnityEngine;

namespace src {

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

        protected virtual void onStart() { }

        protected virtual void onUpdate() { }

        /// <summary>
        /// Writes the object to NBT and returns the tag.
        /// </summary>
        public virtual NbtCompound writeToNbt(NbtCompound tag) {
            tag.Add(new NbtFloat("positionX", this.transform.position.x));
            tag.Add(new NbtFloat("positionY", this.transform.position.y));
            tag.Add(new NbtFloat("positionZ", this.transform.position.z));

            tag.Add(new NbtFloat("rotationY", this.transform.eulerAngles.y));

            return tag;
        }

        /// <summary>
        /// Reads the object from NBT and sets it's state.
        /// </summary>
        public virtual void readFromNbt(NbtCompound tag) {

        }
    }
}
