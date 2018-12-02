using codeshaper.map;
using codeshaper.registry;
using fNbt;
using System.Collections.Generic;
using UnityEngine;

namespace codeshaper.nbt {

    public static class NbtHelper {

        public static void writeList<T>(NbtCompound rootTag, string tagName, List<T> list) where T : INbtSerializable {
            NbtList tagList = new NbtList(tagName, NbtTagType.Compound);
            foreach (T obj in list) {
                NbtCompound t = new NbtCompound();
                obj.writeToNbt(t);
                tagList.Add(t);
            }
            rootTag.Add(tagList);
        }

        public static void readList(NbtCompound rootTag, string tagName, Map map) {
            foreach (NbtCompound compound in rootTag.getList(tagName)) {
                int id = compound.getInt("id");
                RegisteredObject registeredObject = Registry.getObjectFromRegistry(id);
                if (registeredObject != null) {
                    map.spawnEntity(registeredObject, compound);
                }
                else {
                    Debug.Log("Error!  MapObject with an unknown ID of " + id + " was found!  Ignoring!");
                }
            }
        }
    }
}
