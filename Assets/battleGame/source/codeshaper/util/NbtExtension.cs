using fNbt;
using System;
using UnityEngine;

namespace codeshaper.util {

    public static class NbtExtension {

        public static void setTag(this NbtCompound tag, string name, bool value) {
            tag.setTag(name, value ? (byte)1 : (byte)0);
        }

        public static void setTag(this NbtCompound tag, string name, byte value) {
            tag.Add(new NbtByte(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, byte[] value) {
            tag.Add(new NbtByteArray(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, NbtCompound value) {
            tag.Add(new NbtCompound(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, double value) {
            tag.Add(new NbtDouble(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, float value) {
            tag.Add(new NbtFloat(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, int value) {
            tag.Add(new NbtInt(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, int[] value) {
            tag.Add(new NbtIntArray(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, NbtList value) {
            value.Name = name;
            tag.Add(value);
        }

        public static void setTag(this NbtCompound tag, string name, long value) {
            tag.Add(new NbtLong(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, short value) {
            tag.Add(new NbtShort(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, string value) {
            tag.Add(new NbtString(name, value));
        }

        public static void setTag(this NbtCompound tag, string name, Guid value) {
            tag.Add(new NbtString(name, value.ToString()));
        }



        public static bool getBool(this NbtCompound tag, string name, bool defaultValue = false) {
            return tag.getByte(name, defaultValue ? (byte)1 : (byte)0) == 1;
        }

        public static int getByte(this NbtCompound tag, string name, byte defaultValue = 0) {
            NbtByte tag1 = tag.Get<NbtByte>(name);
            if(tag1 == null) {
                return defaultValue;
            } else {
                return tag1.Value;
            }
        }

        public static byte[] getByteArray(this NbtCompound tag, string name) {
            NbtByteArray tag1 = tag.Get<NbtByteArray>(name);
            if (tag1 == null) {
                return new byte[0];
            }
            else {
                return tag1.Value;
            }
        }

        public static NbtCompound getCompound(this NbtCompound tag, string name) {
            NbtCompound tag1 = tag.Get<NbtCompound>(name);
            if (tag1 == null) {
                return new NbtCompound();
            }
            else {
                return tag1;
            }
        }

        public static double getDouble(this NbtCompound tag, string name, double defaultValue = 0) {
            NbtDouble tag1 = tag.Get<NbtDouble>(name);
            if (tag1 == null) {
                return defaultValue;
            }
            else {
                return tag1.Value;
            }
        }

        public static float getFloat(this NbtCompound tag, string name, float defaultValue = 0) {
            NbtFloat tag1 = tag.Get<NbtFloat>(name);
            if (tag1 == null) {
                return defaultValue;
            }
            else {
                return tag1.Value;
            }
        }

        public static int getInt(this NbtCompound tag, string name, int defaultValue = 0) {
            NbtInt tag1 = tag.Get<NbtInt>(name);
            if (tag1 == null) {
                return defaultValue;
            }
            else {
                return tag1.Value;
            }
        }

        public static int[] getIntArray(this NbtCompound tag, string name) {
            NbtIntArray tag1 = tag.Get<NbtIntArray>(name);
            if (tag1 == null) {
                return new int[0];
            }
            else {
                return tag1.Value;
            }
        }

        public static NbtList getList(this NbtCompound tag, string name) {
            NbtList tag1 = tag.Get<NbtList>(name);
            if (tag1 == null) {
                return new NbtList();
            }
            else {
                return tag1;
            }
        }

        public static long getLong(this NbtCompound tag, string name, long defaultValue = 0) {
            NbtLong tag1 = tag.Get<NbtLong>(name);
            if (tag1 == null) {
                return defaultValue;
            }
            else {
                return tag1.Value;
            }
        }

        public static int getShort(this NbtCompound tag, string name, short defaultValue = 0) {
            NbtShort tag1 = tag.Get<NbtShort>(name);
            if (tag1 == null) {
                return defaultValue;
            }
            else {
                return tag1.Value;
            }
        }

        public static string getString(this NbtCompound tag, string name, string defaultValue = "") {
            NbtString tag1 = tag.Get<NbtString>(name);
            if (tag1 == null) {
                return defaultValue;
            }
            else {
                return tag1.Value;
            }
        }

        /// <summary>
        /// Returns null if no tag could be found.
        /// </summary>
        public static Guid? getGuid(this NbtCompound tag, string name, Guid? defaultValue = null) {
            NbtString tag1 = tag.Get<NbtString>(name);
            if (tag1 == null) {
                return defaultValue;
            }
            else {
                return new Guid(tag1.Value);
            }
        }


        /// <summary>
        /// Writes the passed Vector3 to the passed tag, appending "x", "y" and "z" to the prefix to get the tag name.
        /// </summary>
        public static NbtCompound setTag(this NbtCompound tag, string name, Vector3 vector) {
            tag.setTag(name + "_x", vector.x);
            tag.setTag(name + "_y", vector.y);
            tag.setTag(name + "_z", vector.z);
            return tag;
        }

        public static Vector3 getVector3(this NbtCompound tag, string name) {
            return new Vector3(
                tag.getFloat(name + "_x"),
                tag.getFloat(name + "_y"),
                tag.getFloat(name + "_z"));
        }
    }
}
