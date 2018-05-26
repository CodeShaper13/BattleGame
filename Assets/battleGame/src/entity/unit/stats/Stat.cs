using src.util;
using fNbt;
using System;

namespace src.entity.unit.stats {

    public abstract class Stat<T> {

        protected string displayName;
        protected string saveName;
        protected T value;
        private UnitStats unitStats;

        public Stat(UnitStats unitStats, string displayName, string saveName) {
            this.unitStats = unitStats;
            this.displayName = displayName;
            this.saveName = "stat." + saveName;
        }

        public T get() {
            return this.value;
        }

        public virtual void increase(T amount) {
            this.unitStats.dirty = true;
        }

        public abstract void read(NbtCompound tag);

        public abstract void write(NbtCompound tag);

        public override string ToString() {
            return this.displayName + ": " + this.value.ToString();
        }
    }

    public class StatInt : Stat<int> {

        public StatInt(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

        public override void increase(int amount = 1) {
            base.increase(amount);
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

        public StatFloat(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

        public override void increase(float amount) {
            base.increase(amount);
            this.value += amount;
        }


        public override void read(NbtCompound tag) {
            this.value = tag.getFloat(this.saveName);
        }

        public override void write(NbtCompound tag) {
            tag.setTag(this.saveName, this.value);
        }
    }

    public class StatTime : StatFloat {

        public StatTime(UnitStats unitStats, string displayName, string saveName) : base(unitStats, displayName, saveName) { }

        public override string ToString() {
            TimeSpan time = TimeSpan.FromSeconds(this.value);
            string s = time.ToString();
            return this.displayName + ": " + s.Substring(0, s.LastIndexOf("."));
        }
    }
}
