using fNbt;

namespace codeshaper.nbt {

    public interface INbtSerializable {

        void readFromNbt(NbtCompound tag);

        void writeToNbt(NbtCompound tag);
    }
}
