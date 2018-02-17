using UnityEngine;

namespace src.buildings {

    public class BuildingWorkshop : BuildingBase {

        public override int getMaxHealth() {
            return Constants.HEALTH_WORKSHOP;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }
    }
}
