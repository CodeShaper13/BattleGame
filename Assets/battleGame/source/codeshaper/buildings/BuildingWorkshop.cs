using codeshaper.button;
using codeshaper.data;
using UnityEngine;

namespace codeshaper.buildings {

    public class BuildingWorkshop : BuildingQueuedProducerBase {

        public override float getHealthBarHeight() {
            return 2f;
        }

        public override BuildingData getData() {
            return Constants.BD_WORKSHOP;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }

        public override int getButtonMask() {
            return base.getButtonMask() | ActionButton.buildSpecial.mask;
        }

        public override int getQueueSize() {
            return Constants.WORKSHOP_QUEUE_SIZE;
        }
    }
}
