using codeshaper.entity.projectiles;
using codeshaper.util;

namespace codeshaper.entity.unit.task {

    public class TaskIdle : TaskAttackNearby {

        public TaskIdle(UnitBase unit) : base(unit) { }

        public override bool preform() {
            if(Util.isAlive(this.target)) {
                this.func();
            } else {
                this.target = null;
                this.moveHelper.stop();
            }

            return true;
        }

        public override void onDamage(MapObject dealer) {
            if (dealer is Projectile) {
                SidedObjectEntity shooter = ((Projectile)dealer).getShooter();
                if (this.unit.getTeam() != shooter.getTeam()) {
                    this.target = shooter;
                }
            } else if(dealer is SidedObjectEntity) {
                SidedObjectEntity soe = (SidedObjectEntity)dealer;
                if(this.unit.getTeam() != soe.getTeam()) {
                    this.target = soe;
                }
            }
        }

        protected override SidedObjectEntity findTarget() {
            return null; // No target should be found, this is called in the constructor.
        }
    }
}
