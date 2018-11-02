using codeshaper.entity;
using codeshaper.entity.unit;
using codeshaper.registry;
using codeshaper.util;
using fNbt;
using System.Collections.Generic;
using UnityEngine;

namespace codeshaper.buildings {

    public abstract class BuildingQueuedProducerBase : BuildingBase {

        private float trainingProgress;

        public List<RegisteredObject> trainingQueue;

        protected override void onAwake() {
            base.onAwake();

            this.trainingQueue = new List<RegisteredObject>(this.getQueueSize());
        }

        protected override void preformTask() {
            if (this.trainingQueue.Count != 0) {
                bool teamHasRoom = this.getTeam().getTroopCount() <= this.getTeam().getMaxTroopCount();

                if(teamHasRoom) {
                    this.trainingProgress += Time.deltaTime;
                }

                UnitBase nextInQueue = this.trainingQueue[0].getPrefab().GetComponent<UnitBase>();
                if (this.trainingProgress >= nextInQueue.getData().getProductionTime() && teamHasRoom) {
                    Vector2 v = Random.insideUnitCircle * 2f;
                    float i = 1.5f;
                    v.x += (v.x < 0 ? -i : i);
                    v.y += (v.x < 0 ? -i : i);

                    Vector3 pos = this.transform.position + new Vector3(v.x, 0, v.y);
                    RegisteredObject regObj = this.trainingQueue[0];
                    this.trainingQueue.RemoveAt(0);
                    SidedObjectEntity obj = (SidedObjectEntity)this.map.spawnEntity(regObj, pos, Quaternion.Euler(0, Random.Range(0, 359), 0));
                    obj.setTeam(this.getTeam());

                    this.trainingProgress = 0;
                }
            }
        }

        public abstract int getQueueSize();

        /// <summary>
        /// Tries to add an object to the creation queue, retuning true if it was added
        /// or false if the queue was full.
        /// </summary>
        public bool tryAddToQueue(RegisteredObject obj) {
            if (this.trainingQueue.Count < this.getQueueSize()) {
                this.trainingQueue.Add(obj);
                return true;
            }
            else {
                return false;
            }
        }

        public float getTrainingProgress() {
            return this.trainingProgress;
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.trainingProgress = tag.getFloat("trainingProgress");

            NbtList list = tag.getList("trainingQueue");
            foreach (NbtInt integer in list) {
                this.trainingQueue.Add(Registry.getObjectfromRegistry(integer.Value));
            }
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("trainingProgress", this.trainingProgress);

            //TODO check
            NbtList list = new NbtList("trainingQueue", NbtTagType.Int);
            foreach (RegisteredObject ro in this.trainingQueue) {
                list.Add(new NbtInt(ro.getId()));
            }
            tag.Add(list);
        }
    }
}
