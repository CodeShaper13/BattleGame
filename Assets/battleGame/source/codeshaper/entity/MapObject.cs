﻿using fNbt;
using codeshaper.map;
using codeshaper.util;
using System;
using UnityEngine;

namespace codeshaper.entity {

    public abstract class MapObject : MonoBehaviour {

        /// <summary> A reference to the Map object representing the state of this map. </summary>
        [HideInInspector]
        public Map map;

        /// <summary>
        /// Objects marks as immutable are special and can not be destroyed via a button or edited (rotated, etc.)
        /// </summary>
        [SerializeField] // Used so it can be set in the inspecter.
        private bool immutable;
        private Guid guid;

        private void Awake() {
            this.guid = Guid.NewGuid();

            this.onAwake();
        }

        private void Start() {
            this.map = Map.getInstance();

            this.onStart();
        }

        private void Update() {
            this.onUpdate();
        }

        private void LateUpdate() {
            this.onLateUpdate();
        }

        protected virtual void onAwake() { }

        protected virtual void onStart() { }

        public virtual void onConstruct() { }

        protected virtual void onUpdate() { }

        protected virtual void onLateUpdate() { }

        /// <summary>
        /// Returns this objects Guid.
        /// </summary>
        public Guid getGuid() {
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
            return this.transform.position;
        }

        /// <summary>
        /// Reads the object from NBT and sets it's state.
        /// </summary>
        public virtual void readFromNbt(NbtCompound tag) {
            this.transform.position = tag.getVector3("position");
            this.transform.eulerAngles = tag.getVector3("eulerRotation");
            this.immutable = tag.getBool("isImmutable");
            this.guid = new Guid(tag.getString("guid"));
        }

        /// <summary>
        /// Writes the object to NBT and returns the tag.
        /// </summary>
        public virtual void writeToNbt(NbtCompound tag) {
            tag.setTag("position", this.transform.position);
            tag.setTag("eulerRotation", this.transform.eulerAngles);
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
