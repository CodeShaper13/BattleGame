using src.util;
using fNbt;

namespace src.entity.unit.statistics {

    public abstract class Stat<T> {

        protected string displayName;
        protected string saveName;
        protected T value;

        public Stat(string displayName, string saveName) {
            this.displayName = displayName;
            this.saveName = saveName;
        }

        public T get() {
            return this.value;
        }

        public abstract void increase(T amount);

        public abstract void read(NbtCompound tag);

        public abstract void write(NbtCompound tag);

        public string getDisplayName() {
            return this.displayName;
        }
    }

    public class StatInt : Stat<int> {

        public StatInt(string displayName, string saveName) : base(displayName, saveName) { }

        public override void increase(int amount) {
            this.value += amount;
        }

        public override void read(NbtCompound tag) {
            this.value = tag.getInt(this.saveName);
        }

        public override void write(NbtCompound tag) {
            tag.setTag(this.saveName, this.value);
        }
    }

    public class StatFloat : Stat<float> {

        public StatFloat(string displayName, string saveName) : base(displayName, saveName) { }

        public override void increase(float amount) {
            this.value += amount;
        }

        public override void read(NbtCompound tag) {
            this.value = tag.getFloat(this.saveName);
        }

        public override void write(NbtCompound tag) {
            tag.setTag(this.saveName, this.value);
        }
    }
}
