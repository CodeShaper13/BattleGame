using fNbt;
using src.data;
using UnityEngine;

namespace src.buildings.harvestable {

    public class HarvestableObject : MapObject {

        public EnumHarvestableType type;
        private float sizeRadius;

        private Health health;

        protected override void onStart() {
            base.onStart();

            this.sizeRadius = this.func();

            this.health = this.GetComponentInChildren<Health>();

            this.health.setMaxHealth(this.getTotalResourceYield());

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
        public bool harvest(troop.UnitBuilder builder) {
            int remainingSpace = Constants.BUILDER_MAX_CARRY - builder.resources;
            int amountToHarvest = Mathf.Min(Constants.BUILDER_COLLECT_PER_STRIKE, remainingSpace);
            builder.resources += amountToHarvest;
            if (this.health.setHealth(this.health.getHealth() - amountToHarvest)) {
                //TODO destroy/particle effect.
                map.removeHarvestableObject(this);
                GameObject.Destroy(this.gameObject);
                return true;
            }

            return false;
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
                case EnumHarvestableType.TREE: return 100;
                case EnumHarvestableType.BIG_TREE: return 200;
                case EnumHarvestableType.ROCK: return 150;
                case EnumHarvestableType.BIG_ROCK: return 300;
            }
            return 0;
        }

        public override NbtCompound writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.Add(new NbtInt("health", this.health.getHealth()));

            return tag;
        }
    }
}
