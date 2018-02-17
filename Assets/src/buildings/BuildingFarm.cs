using UnityEngine;

namespace src.buildings {

    public class BuildingFarm : BuildingBase {

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }

        public override int getMaxHealth() {
            return Constants.HEALTH_FARM;
        }
    }
}
