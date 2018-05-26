using src.button;
using src.data;
using src.entity.unit;
using src.registry;
using System.Collections.Generic;
using UnityEngine;
using fNbt;
using src.util;

namespace src.buildings {

    public class BuildingTrainingHouse : BuildingBase {

        private Queue<RegisteredObject> trainingQueue;
        private float trainingProgress;

        protected override void onAwake() {
            base.onAwake();

            this.trainingQueue = new Queue<RegisteredObject>(Constants.TRAINING_CAMP_QUEUE_SIZE);
        }

        protected override void onUpdate() {
            base.onUpdate();

            if(this.trainingQueue.Count != 0) {
                this.trainingProgress += Time.deltaTime;
                if(this.trainingProgress >= Constants.TIME_TO_TRAIN) {
                    Vector2 v = Random.insideUnitCircle * 2f;
                    float i = 1.5f;
                    v.x += (v.x < 0 ? -i : i);
                    v.y += (v.x < 0 ? -i : i);

                    Vector3 pos = this.transform.position + new Vector3(v.x, 0, v.y);
                    UnitBase unit = (UnitBase)this.map.spawnEntity(this.trainingQueue.Dequeue(), pos, Quaternion.identity);
                    unit.setTeam(this.getTeam());

                    this.trainingProgress = 0;
                }
            }
        }

        public override float getHealthBarHeight() {
            return 2f;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }

        public override BuildingData getData() {
            return Constants.BD_TRAINING_HOUSE;
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.train.mask;
        }

        /// <summary>
        /// Tries to add a unit to the training queue, retuning true if it was added
        /// or false if the queue was full.
        /// </summary>
        public bool addToQueue(RegisteredObject obj) {
            if(this.trainingQueue.Count < 3) {
                this.trainingQueue.Enqueue(obj);
                return true;
            } else {
                return false;
            }
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.trainingProgress = tag.getFloat("trainingProgress");
            NbtList list = tag.getList("trainingQueue");
            foreach (NbtInt integer in list) {
                this.trainingQueue.Enqueue(Registry.getObjectfromRegistry(integer.Value));
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
