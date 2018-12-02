using fNbt;
using codeshaper.entity.unit;
using codeshaper.util;
using System;
using UnityEngine;
using codeshaper.nbt;

namespace codeshaper.entity.projectiles {

    public class Projectile : MapObject {

        private const float PROJECTILE_SPEED = 12f;

        private int damage;
        private LivingObject target;
        /// <summary> Keeps track of who shot the projectile, used for statistics.  Null if shot by a building. </summary>
        private SidedObjectEntity shooter;

        private void Update() {
            if(this.target == null || !(this.target)) {
                this.destroyProjectile();
            } else {
                // Move projectile towards target.
                this.transform.LookAt(this.target.transform.position);
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.target.transform.position, Projectile.PROJECTILE_SPEED * Time.deltaTime);


                // Damge target if the projectile is close enough.
                if (MathHelper.inRange(this.transform.position, this.target.transform.position, 0.25f)) {
                    bool isDead = this.target.damage(this, this.damage);
                    if(isDead && shooter is UnitBase) { // Not true for buildings like the tower.
                        ((UnitBase)this.shooter).unitStats.unitsKilled.increase();
                    }
                    this.destroyProjectile();
                }
            }
        }

        private void destroyProjectile() {
            GameObject.Destroy(this.gameObject);
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.damage = tag.getInt("damageDelt");
            this.target = (LivingObject)this.map.findMapObjectFromGuid(tag.getGuid("target"));
            Guid guid = tag.getGuid("shooter");
            this.shooter = (SidedObjectEntity)(guid != Guid.Empty ? this.map.findMapObjectFromGuid(guid) : null);
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("damageDelt", this.damage);
            tag.setTag("shooter", Util.isAlive(this.shooter) ? this.shooter.getGuid() : Guid.Empty);
        }

        /// <summary>
        /// Returns the object that shot this projectile.  This should never be null.
        /// </summary>
        public SidedObjectEntity getShooter() {
            return this.shooter;
        }

        public void setProjectileInfo(SidedObjectEntity shooter, int damage, LivingObject target) {
            this.shooter = shooter;
            this.damage = damage;
            this.target = target;
        }
    }
}
