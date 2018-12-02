using codeshaper.nbt;
using UnityEngine;
using fNbt;
using System.Collections.Generic;
using codeshaper.util;
using codeshaper.map.chunkLoader;
using codeshaper.debug;
using codeshaper.entity;
using System;

namespace codeshaper.map.chunk {

    [DisallowMultipleComponent]
    public class Chunk : MonoBehaviour, IDrawDebug {

        public const int SIZE = 50;
        public const float SIZEf = SIZE;

        public ChunkPos chunkPos;

        private Map map;
        public Bounds chunkBounds;

        public List<MapObject> mapObjects;

        private Transform HOLDER;
        //private Transform BUILDING_HOLDER;

        private void Awake() {
            this.mapObjects = new List<MapObject>();

            this.HOLDER = this.createWrapper("OBJ");
            //this.BUILDING_HOLDER = this.createWrapper("BUILDING");
        }

        public void onUpdate() {
            for(int i = 0; i < this.mapObjects.Count; i++) {
                this.mapObjects[i].onUpdate();
            }
        }

        public void drawDebug() {
            const float f = Chunk.SIZEf / 2;
            GLDebug.DrawSquare(this.chunkPos.toWorldSpaceVector() + new Vector3(f, 0, f), Quaternion.identity, new Vector3(SIZE, SIZE, SIZE), Colors.magenta);

            foreach(MapObject obj in this.mapObjects) {
                obj.drawDebug();
            }
        }

        /// <summary>
        /// Acts like a constructor of a chunk.
        /// </summary>
        public void initChunk(Map map, NewChunkInstructions instructions) {
            this.map = map;
            this.chunkPos = instructions.chunkPos;
            this.transform.position = this.chunkPos.toWorldSpaceVector();
            //this.isReadOnly = instructions.isReadOnly;
            float radius = 7f;
            Vector3 worldPos = this.chunkPos.toWorldSpaceVector();
            this.chunkBounds = new Bounds(new Vector3(radius + worldPos.x, radius + worldPos.y, radius + worldPos.z), new Vector3(Chunk.SIZE, Chunk.SIZE, Chunk.SIZE));
            #if UNITY_EDITOR
                this.name = "Chunk" + this.chunkPos.ToString();
            #endif
        }

        /// <summary>
        /// Resets the chunk, clearing out fields and preparing it to be used again.
        /// </summary>
        public void resetChunk() {
            this.mapObjects.Clear();
            foreach(Transform t in this.HOLDER) {
                GameObject.Destroy(t.gameObject);
            }
        }

        public void readFromNbt(NbtCompound tag) {
            this.chunkPos = new ChunkPos(tag.getInt("chunkX"), tag.getInt("chunkZ"));

            NbtHelper.readList(tag, "staticObjects", this.map);

            NbtHelper.readList(tag, "mapObjects", this.map);
        }

        public void writeToNbt(NbtCompound tag, bool deleteEntities) {
            tag.setTag("chunkX", this.chunkPos.x);
            tag.setTag("chunkZ", this.chunkPos.z);

            NbtHelper.writeList(tag, "staticObjects", this.mapObjects);

            // Entites.
            MapObject entity;
            List<MapObject> entitiesInChunk = new List<MapObject>();
            int i, x, y, z;
            for (i = this.map.mapObjects.Count - 1; i >= 0; i--) {
                entity = this.map.mapObjects[i];
                //if (!(entity is EntityPlayer)) {
                    x = MathHelper.floor((int)entity.transform.position.x / Chunk.SIZEf);
                    y = MathHelper.floor((int)entity.transform.position.y / Chunk.SIZEf);
                    z = MathHelper.floor((int)entity.transform.position.z / Chunk.SIZEf);

                    if (x == this.chunkPos.x && z == this.chunkPos.z) {
                        this.map.mapObjects.Remove(entity);
                        entitiesInChunk.Add(entity);
                    }
                //}
            }
            NbtList nbtMapObjList = new NbtList("mapObjects", NbtTagType.Compound);
            for (i = 0; i < entitiesInChunk.Count; i++) {
                entity = entitiesInChunk[i];
                NbtCompound tagCompound = new NbtCompound();
                entity.writeToNbt(tagCompound);
                nbtMapObjList.Add(tagCompound);

                if (deleteEntities) {
                    GameObject.Destroy(entity.gameObject);
                }
            }
            tag.Add(nbtMapObjList);
        }

        public MapObject findObjectFromGuid(Guid guid) {
            foreach (MapObject obj in this.mapObjects) {
                if (obj.getGuid() == guid) {
                    return obj;
                }
            }
            return null;
        }

        public void func02(MapObject obj) {
            this.mapObjects.Add(obj);
            obj.transform.parent = this.HOLDER;
            //if(obj is BuildingBase) {
            //    this.buildings.Add((BuildingBase)obj);
            //    obj.transform.parent = this.BUILDING_HOLDER;
            //} else {
            //    this.harvestableObjects.Add((HarvestableObject)obj);
            //    obj.transform.parent = this.HOLDER;
            //}
        }

        private Transform createWrapper(string s) {
            Transform t = new GameObject(s + "_HOLDER").transform;
            t.SetParent(this.transform);
            return t;
        }
    }
}
