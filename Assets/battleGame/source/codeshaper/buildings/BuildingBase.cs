using fNbt;
using codeshaper.button;
using codeshaper.data;
using codeshaper.entity;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.buildings {

    public abstract class BuildingBase : SidedObjectEntity {

        /// <summary>
        /// True if the building is being constructed.  Buildings that
        /// are still being built can not function.
        /// </summary>
        private bool constructing;
        private float buildProgress;

        private int targetRotation = 0;

        protected override void onUpdate() {
            if(!this.constructing) {
                this.preformTask();
            }

            // Debug outline.
            if(Main.DEBUG) {
                Vector2 v = this.getFootprintSize();
                GLDebug.DrawCube(this.transform.position, Quaternion.identity, new Vector3(v.x, 0.35f, v.y), new Color(0.5f, 0, 0.5f));
            }

            // Rotate the building slowly if it is being rotated.
            if(this.transform.eulerAngles.y != this.targetRotation) {
                this.transform.rotation = Quaternion.RotateTowards(
                    this.transform.rotation,
                    Quaternion.Euler(0, this.targetRotation, 0),
                    250 * Time.deltaTime);
            }
        }

        public override int getButtonMask() {
            int mask = base.getButtonMask();
            if(!(this is BuildingWall)) {
                mask |= ActionButton.buildingRotate.getMask();
            }
            return mask;
        }

        /// <summary>
        /// Sets the building to be currently being constructed by a builder.
        /// </summary>
        public void setConstructing() {
            this.constructing = true;
            this.buildProgress = 1;
        }

        public bool isConstructing() {
            return this.constructing;
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
        protected virtual void preformTask() { }

        /// <summary>
        /// Returns the size of the building as a Vector2 of (width, height).
        /// </summary>
        public abstract Vector2 getFootprintSize();

        public abstract BuildingData getData();

        public override int getMaxHealth() {
            return this.getData().getMaxHealth();
        }

        /// <summary>
        /// Used to continue to construct a building or to repair it.
        /// Returns true if the building was finished on this call.
        /// </summary>
        public bool increaseConstructed(bool deductResources) {
            this.buildProgress += (Constants.CONSTRUCT_RATE * Time.deltaTime);
            bool finished = false;

            if((int)this.buildProgress >= this.getMaxHealth()) {
                this.constructing = false;
                this.buildProgress = 0;
                finished = true;
            }

            if(deductResources && (int)buildProgress > this.getHealth()) {
                this.getTeam().reduceResources(1);
            }

            this.setHealth((int)this.buildProgress);

            return finished;
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

            this.constructing = tag.getBool("isBuilding");
            this.buildProgress = tag.getFloat("progress");
            this.targetRotation = tag.getInt("targetRotation");
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("isBuilding", this.constructing);
            tag.setTag("progress", this.buildProgress);
            tag.setTag("targetRotation", this.targetRotation);
        }
    }
}
