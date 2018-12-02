using fNbt;
using codeshaper.data;
using codeshaper.entity.unit;
using UnityEngine;
using codeshaper.registry;
using codeshaper.entity.projectiles;
using codeshaper.entity;
using System;
using codeshaper.nbt;

namespace codeshaper.buildings {

    public class BuildingTower : BuildingBase {

        private UnitBase target;
        private float fireCooldown;

        public override void onUpdate() {
            base.onUpdate();

            this.fireCooldown -= Time.deltaTime;
        }

        protected override void preformTask() {
            if(this.target == null || !(this.target)) {
                this.findTarget();
            }

            if (this.target != null && this.fireCooldown <= 0) {
                Projectile arrow = (Projectile)this.map.spawnEntity(Registry.projectileArrow, this.transform.position + new Vector3(0, 5.9f, 0), Quaternion.identity);
                arrow.setProjectileInfo(this, Constants.BUILDING_TOWER_DAMAGE, this.target);
                this.fireCooldown = Constants.BUILDING_TOWER_FIRE_SPEED;
            }
        }

        public override float getHealthBarHeight() {
            return 6.5f;
        }

        public override Vector2 getFootprintSize() {
            return new Vector2(3, 3);
        }

        public override BuildingData getData() {
            return Constants.BD_TOWER;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.fireCooldown = tag.getFloat("fireCooldown");
            MapObject obj = this.map.findMapObjectFromGuid(tag.getGuid("targetGuid"));
            if(obj is UnitBase) {
                this.target = (UnitBase)obj;
            } else {
                this.target = null;
            }
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("fireCooldown", this.fireCooldown);
            tag.setTag("targetGuid", this.target.getGuid());
        }

        /// <summary>
        /// Trys to find a target.
        /// </summary>
        private void findTarget() {
            if (this.target == null || this.target.isDead() || Vector3.Distance(this.transform.position, this.target.transform.position) >= Constants.BUILDING_TOWER_FIRE_RANGE) {
                this.target = this.findUnit();
            }
        }

        private UnitBase findUnit() {
            UnitBase obj = null;
            float f = float.PositiveInfinity;
            foreach (SidedObjectEntity s in this.map.findMapObjects(this.getTeam().predicateOtherTeam)) {
                if (s is UnitBase) {
                    float dis = Vector3.Distance(this.transform.position, s.transform.position);
                    if ((dis < f) && dis < Constants.BUILDING_TOWER_SEE_RANGE) {
                        obj = (UnitBase)s;
                        f = dis;
                    }
                }
            }
            return obj;
        }
    }
}
