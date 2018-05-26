using fNbt;

namespace src.entity.projectiles {

    public class Projectile : SidedObjectBase {

        private void Update() {
            
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);
        }
    }
}
