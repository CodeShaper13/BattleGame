using codeshaper.button;
using codeshaper.data;
using UnityEngine;

namespace codeshaper.buildings {

    public class BuildingTrainingHouse : BuildingQueuedProducerBase {

        public override float getHealthBarHeight() {
            return 3f;
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

        public override int getQueueSize() {
            return Constants.TRAINING_HOUSE_QUEUE_SIZE;
        }
    }
}
