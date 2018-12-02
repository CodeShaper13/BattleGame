using codeshaper.data;
using codeshaper.entity.projectiles;
using codeshaper.registry;
using UnityEngine;

namespace codeshaper.entity.unit.task.attack {

    public class AttackArrow : AttackBase {

        public AttackArrow(UnitBase unit) : base(unit) { }

        public override bool inRangeToAttack(SidedObjectEntity target) {
            return Vector3.Distance(this.unit.getPos(), target.getPos()) <= Constants.AI_ARCHER_SHOOT_RANGE;
        }

        protected override void preformAttack(SidedObjectEntity target) {
            Projectile arrow = (Projectile)this.unit.map.spawnEntity(Registry.projectileArrow, this.unit.transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            arrow.setProjectileInfo(this.unit, this.unit.unitStats.getAttack(), target);
        }
    }
}
