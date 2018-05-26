using src.data;
using UnityEngine;

namespace src.buildings {

    public class BuildingTower : BuildingBase {

        public override float getHealthBarHeight() {
            return 2f;
        }

        public override Vector2 getFootprintSize() {
            return new Vector2(3, 3);
        }

        public override BuildingData getData() {
            return Constants.BD_TOWER;
        }
    }
}
