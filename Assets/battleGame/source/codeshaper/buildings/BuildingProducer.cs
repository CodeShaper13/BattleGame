using codeshaper.data;
using codeshaper.util;
using fNbt;
using UnityEngine;

namespace codeshaper.buildings {

    public class BuildingProducer : BuildingBase, IResourceHolder {

        private float time;
        private int heldResources;

        public override float getHealthBarHeight() {
            return 3f;
        }

        public override BuildingData getData() {
            return Constants.BD_PRODUCER;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }

        public override int getButtonMask() {
            return base.getButtonMask();
        }


        protected override void preformTask() {
            if (this.heldResources < Constants.BUILDING_PRODUCER_MAX_HOLD) {
                this.time += (Time.deltaTime * Constants.BUILDING_PRODUCER_RATE);

                if (this.time > 0) {
                    this.time = 0;
                    this.heldResources += 1;
                }
            }
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.time = tag.getFloat("produceTime");
            this.heldResources = tag.getInt("heldResources");
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("produceTime", this.time);
            tag.setTag("heldResources", this.heldResources);
        }

        public int getHeldResources() {
            return this.heldResources;
        }

        public int getHoldLimit() {
            return Constants.BUILDING_PRODUCER_MAX_HOLD;
        }
    }
}
