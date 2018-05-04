using System;
using fNbt;
using src.data;
using UnityEngine;

namespace src.buildings {

    public abstract class BuildingBase : SidedObjectEntity {

        /// <summary> True if the building is being constructed.  Buildings that are still being built can not function. </summary>
        private bool isBuilding;
        private float buildProgress;
        private BuildingData data;

        protected override void onAwake() {
            base.onAwake();

            this.data = this.getBuildingData();
        }

        protected override void onStart() {
            base.onStart();
        }

        protected override void onUpdate() {
            base.onUpdate();

            if(this.isBuilding) {
                this.buildProgress += (Constants.CONSTRUCT_RATE * Time.deltaTime);

                if(this.buildProgress >= this.getMaxHealth()) {
                    this.isBuilding = false;
                    this.buildProgress = 0;
                }
                this.setHealth((int)this.buildProgress);
            } else {
                this.preformTask();
            }
        }

        /// <summary>
        /// Sets the building to be currently being constructed by a builder.
        /// </summary>
        public void setConstructing() {
            this.isBuilding = true;
        }

        public override float getSizeRadius() {
            Vector2 v = this.getFootprintSize();
            return (Mathf.Max(v.x, v.y) / 2);
        }

        public override void onDeathCallback() {
            base.onDeathCallback();
            GameObject.Destroy(this.gameObject);
        }

        /// <summary>
        /// Called every frame for the building to preform it's task, if it has any.
        /// </summary>
        public virtual void preformTask() { }

        /// <summary>
        /// Returns the size of the building as a Vector2 of (width, height).
        /// </summary>
        public abstract Vector2 getFootprintSize();

        public abstract BuildingData getBuildingData();

        public override int getMaxHealth() {
            return this.data.getHealth();
        }

        public int getCost() {
            return this.data.getCost();
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.Add(new NbtByte("isBuilding", this.isBuilding ? (byte)1 : (byte)0));
            tag.Add(new NbtFloat("progress", this.buildProgress));

            return tag;
        }
    }
}
