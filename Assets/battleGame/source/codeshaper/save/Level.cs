using fNbt;

namespace Assets.battleGame.src.save {

    public class Level {

        public int sceneId;

        public NbtCompound writeToNbt() {
            NbtCompound tag = new NbtCompound("world");
            //tag.Add(new NbtInt("seed", this.seed));
            //NbtHelper.writeDirectVector3(tag, this.spawnPos, "spawn");
            //tag.Add(new NbtInt("worldType", this.worldType));
            //tag.Add(new NbtLong("lastLoaded", this.lastLoaded.ToBinary()));
            return tag;
        }

        public void readFromNbt(NbtCompound tag) {

        }
    }
}
