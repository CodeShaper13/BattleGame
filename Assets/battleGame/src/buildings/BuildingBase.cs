using fNbt;
using src.data;
using src.entity;
using src.util;
using UnityEngine;

namespace src.buildings {

    public abstract class BuildingBase : SidedObjectEntity {

        /// <summary> True if the building is being constructed.  Buildings that are still being built can not function. </summary>
        public bool isConstructing;
        public float buildProgress;

        protected override void onUpdate() {
            base.onUpdate();

            if(this.isConstructing) {
                this.buildProgress += (Constants.CONSTRUCT_RATE * Time.deltaTime);

                if((int)this.buildProgress > this.getMaxHealth()) {
                    this.isConstructing = false;
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
            this.isConstructing = true;
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

        public abstract BuildingData getData();

        public override int getMaxHealth() {
            return this.getData().getMaxHealth();
        }

        public override float getHealthBarHeight() {
            return 2f;
        }

        public int getCost() {
            return this.getData().getCost();
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.isConstructing = tag.getByte("isBuilding") == 1;
            this.buildProgress = tag.getFloat("progress");
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("isBuilding", this.isConstructing ? (byte)1 : (byte)0);
            tag.setTag("progress", this.buildProgress);

            return tag;
        }
    }
}
