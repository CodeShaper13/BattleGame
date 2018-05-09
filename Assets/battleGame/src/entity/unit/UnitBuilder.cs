﻿using fNbt;
using src.buildings;
using src.button;
using src.data;
using src.util;

namespace src.entity.unit {

    public class UnitBuilder : UnitBase<UnitBuilder> {

        public int resources;

        private BuildingBase building;

        /// <summary>
        /// Sets the held resources to 0 and returns what
        /// the builder is carrying.
        /// </summary>
        public int deposite() {
            int i = this.resources;
            this.resources = 0;
            return i;
        }

        protected override void onUpdate() {
            base.onUpdate();
        }

        public bool canCarryMore() {
            return this.resources < Constants.BUILDER_MAX_CARRY;
        }

        public override EntityData getData() {
            return Constants.ED_BUILDER;
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.builderBuild.mask | ActionButton.harvestResources.mask;
        }

        public override bool enableActionButton() {
            return !this.building.isConstructing;
        }

        public void setBuilding(BuildingBase building) {
            this.building = building;
            this.setTask(null);
            this.setDestination(building.transform.position);
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.resources = tag.getInt("builderResources");
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("builderResources", this.resources);

            return tag;
        }
    }
}