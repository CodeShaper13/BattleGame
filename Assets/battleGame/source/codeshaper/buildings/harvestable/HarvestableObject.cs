using fNbt;
using codeshaper.data;
using codeshaper.entity.unit;
using UnityEngine;
using codeshaper.entity;

namespace codeshaper.buildings.harvestable {

    [DisallowMultipleComponent]
    public class HarvestableObject : LivingObject {

        public EnumHarvestableType type;
        private float sizeRadius;

        protected override void onStart() {
            base.onStart();

            this.sizeRadius = this.func();
            this.map.addHarvestableObject(this);
        }

        private float func() {
            CapsuleCollider cc = this.GetComponent<CapsuleCollider>();
            if (cc != null) {
                return cc.radius;
            }

            SphereCollider sc = this.GetComponent<SphereCollider>();
            if (sc != null) {
                return sc.radius;
            }

            BoxCollider bc = this.GetComponent<BoxCollider>();
            if(bc != null) {
                return Mathf.Max(bc.size.x, bc.size.z) / 2;
            }

            return 0;
        }

        /// <summary>
        /// Returns true if the object was destroyed.
        /// </summary>
        public bool harvest(UnitBuilder builder) {
            int remainingSpace = Constants.BUILDER_MAX_CARRY - builder.getResources();
            int amountToHarvest = Mathf.Min(Constants.BUILDER_COLLECT_PER_STRIKE, remainingSpace);
            builder.increaseResources(amountToHarvest);

            return this.damage(amountToHarvest);
        }

        public override void onDeathCallback() {
            base.onDeathCallback();

            //TODO destroy/particle effect.

            this.map.removeHarvestableObject(this);
        }

        /// <summary>
        /// Returns the radius of the harvestable object.
        /// </summary>
        public float getSizeRadius() {
            return this.sizeRadius * 2f;
        }

        private int getTotalResourceYield() {
            switch(this.type) {
                case EnumHarvestableType.BUSH: return 50;
                case EnumHarvestableType.DEAD_TREE: return 100;
                case EnumHarvestableType.TALL_CACTUS: return 200;
                case EnumHarvestableType.ROCK: return 150;
                case EnumHarvestableType.SKULL: return 300;
            }
            return 0;
        }

        //TODO read type?

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.Add(new NbtInt("type", (int)this.type));
        }

        public override int getMaxHealth() {
            return this.getTotalResourceYield();
        }

        public override float getHealthBarHeight() {
            switch (this.type) {
                case EnumHarvestableType.BUSH: return 1f;
                case EnumHarvestableType.DEAD_TREE: return 2.65f;
                case EnumHarvestableType.TALL_CACTUS: return 2.75f;
                case EnumHarvestableType.ROCK: return 1f;
                case EnumHarvestableType.SKULL: return 1f;
            }
            return 0f;
        }
    }
}
