using src.data;
using UnityEngine;

namespace src.buildings {

    public class BuildingTower : BuildingBase {

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }

        public override BuildingData getBuildingData() {
            return Constants.BD_TOWER;
        }
    }
}
