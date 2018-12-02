using fNbt;
using codeshaper.map;
using System;
using UnityEngine;
using codeshaper.registry;
using codeshaper.nbt;
using codeshaper.debug;

namespace codeshaper.entity {

    [DisallowMultipleComponent]
    public abstract class MapObject : MonoBehaviour, INbtSerializable, IDrawDebug {

        /// <summary> A reference to the Map object representing the state of this map. </summary>
        [HideInInspector]
        public Map map;
        /// <summary>
        /// Objects marks as immutable are special and can not be destroyed via a button or edited (rotated, etc.)
        /// </summary>
        [SerializeField] // Used so it can be set in the inspecter.
        private bool immutable;
        private Guid guid;
        private Transform transformRef;

        private void Awake() {
            this.transformRef = this.transform;
            this.map = Map.instance();

            //#if UNITY_EDITOR
            //    // Helper bit to clean up the Hierarchy a bit.
            //    if (this.transformRef.parent == null) {
            //        this.map.placeInWrapper(this);
            //    }
            //#endif

            this.onAwake();
        }

        private void Start() {
            this.onStart();
        }

        protected virtual void onAwake() { }

        protected virtual void onStart() { }

        /// <summary>
        /// Called when a MapObject is spawned and enters the map.  This is not called on objects that are loaded from NBT.
        /// </summary>
        public virtual void onInitialConstruct() {
            this.guid = Guid.NewGuid();
        }

        /// <summary>
        /// Called every frame that the game is not paused.  Use this to update the object.
        /// </summary>
        public virtual void onUpdate() { }

        public virtual void drawDebug() { }

        /// <summary>
        /// Returns this objects Guid.
        /// </summary>
        public Guid getGuid() {
            if(this.guid == Guid.Empty) {
                this.guid = Guid.NewGuid();
            }
            return this.guid;
        }

        /// <summary>
        /// Returns true if the object is immutable.
        /// </summary>
        public bool isImmutable() {
            return this.immutable;
        }

        /// <summary>
        /// Easier way to get the position vector of this object.
        /// </summary>
        public Vector3 getPos() {
            return this.transformRef.position;
        }

        /// <summary>
        /// Reads the object from NBT and sets it's state.
        /// </summary>
        public virtual void readFromNbt(NbtCompound tag) {
            // Don't read id from NBT.

            this.transform.position = tag.getVector3("position");
            this.transform.eulerAngles = tag.getVector3("eulerRotation");
            this.transform.localScale = tag.getVector3("localScale", Vector3.one);
            this.immutable = tag.getBool("isImmutable");
            this.guid = new Guid(tag.getString("guid"));
        }

        /// <summary>
        /// Writes the object to NBT.
        /// </summary>
        public virtual void writeToNbt(NbtCompound tag) {
            tag.setTag("id", Registry.getIdFromObject(this));

            tag.setTag("position", this.transform.position);
            tag.setTag("eulerRotation", this.transform.eulerAngles);
            tag.setTag("localScale", this.transform.localScale);
            tag.setTag("isImmutable", this.immutable);
            tag.setTag("guid", this.guid.ToString());
        }

        public static bool operator ==(MapObject lhs, MapObject rhs) {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(MapObject lhs, MapObject rhs) {
            return !Equals(lhs, rhs);
        }

        public override bool Equals(object obj) {
            if (obj == null || this.GetType() != obj.GetType()) {
                return false;
            }

            return this.getGuid().Equals(((MapObject)obj).getGuid());
        }

        public override int GetHashCode() {
            return this.guid.GetHashCode();
        }
    }
}
