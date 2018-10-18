using fNbt;
using codeshaper.button;
using codeshaper.data;
using codeshaper.entity;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.buildings {

    public abstract class BuildingBase : SidedObjectEntity {

        /// <summary> True if the building is being constructed.  Buildings that are still being built can not function. </summary>
        public bool isConstructing;
        private float buildProgress;

        private int targetRotation = 0;

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


            // Rotate the building slowly.
            if(this.transform.eulerAngles.y != this.targetRotation) {
                this.transform.rotation = Quaternion.RotateTowards(
                    this.transform.rotation,
                    Quaternion.Euler(0, this.targetRotation, 0),
                    200 * Time.deltaTime);
            }
        }

        public override int getButtonMask() {
            return ActionButton.buildingRotate.mask | base.getButtonMask();
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

            // Hacky way to remove unit from the party.
            CameraMover cm = CameraMover.instance();
            if (cm.selectedBuilding == this) {
                cm.selectedBuilding = null;
            }

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

        public override void setOutlineVisibility(bool visible) {


            base.setOutlineVisibility(visible);
        }

        public int getCost() {
            return this.getData().getCost();
        }

        public void rotateBuilding() {
            this.targetRotation += 90;
            if(this.targetRotation >= 360) {
                this.targetRotation = 0;
            }
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.isConstructing = tag.getBool("isBuilding");
            this.buildProgress = tag.getFloat("progress");
            this.targetRotation = tag.getInt("targetRotation");
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("isBuilding", this.isConstructing);
            tag.setTag("progress", this.buildProgress);
            tag.setTag("targetRotation", this.targetRotation);
        }
    }
}
