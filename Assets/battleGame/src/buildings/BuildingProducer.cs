using src.data;
using UnityEngine;

namespace src.buildings {

    public class BuildingProducer : BuildingBase {

        private float time;
        private int heldResources;

        public override float getHealthBarHeight() {
            return 2f;
        }

        public override BuildingData getData() {
            return Constants.BD_PRODUCER;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }

        protected override void onUpdate() {
            base.onUpdate();

            this.time += (Time.deltaTime * Constants.PRODUCER_RATE);

            if(this.time > 0) {
                this.time = 0;
                this.heldResources += 1;
            }
        }
    }
}
