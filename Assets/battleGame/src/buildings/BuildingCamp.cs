using src.data;
using UnityEngine;

namespace src.buildings {

    public class BuildingCamp : BuildingBase {

        public override BuildingData getBuildingData() {
            return Constants.BD_CAMP;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }
    }
}
