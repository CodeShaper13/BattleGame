using fNbt;
using codeshaper.buildings;
using codeshaper.button;
using codeshaper.data;
using codeshaper.nbt;

namespace codeshaper.entity.unit {

    public class UnitBuilder : UnitBase<UnitBuilder>, IResourceHolder {

        private int heldResources;

        /// <summary>
        /// Sets the held resources to 0 and returns what
        /// the builder is carrying.
        /// </summary>
        public int deposite() {
            int i = this.heldResources;
            this.heldResources = 0;
            return i;
        }

        public void increaseResources(int amount) {
            // TEMP resources are deposited directly into the general bank.
            //this.resources += amount;
            this.getTeam().increaseResources(amount);

            this.unitStats.resourcesCollected.increase(amount);
        }

        public override EntityBaseStats getData() {
            return Constants.ED_BUILDER;
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.builderBuild.getMask() | ActionButton.harvestResources.getMask() | ActionButton.repair.getMask();
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.heldResources = tag.getInt("builderResources");
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("builderResources", this.heldResources);
        }

        public int getHeldResources() {
            return this.heldResources;
        }

        public int getHoldLimit() {
            return Constants.BUILDER_MAX_CARRY;
        }

        public bool canHoldMore() {
            return this.heldResources < this.getHoldLimit();
        }
    }
}
