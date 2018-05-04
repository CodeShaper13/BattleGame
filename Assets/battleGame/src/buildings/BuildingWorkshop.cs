using src.data;
using UnityEngine;

namespace src.buildings {

    // builds special vehicles and weapons?
    public class BuildingWorkshop : BuildingBase {

        public override BuildingData getBuildingData() {
            return Constants.BD_WORKSHOP;
        }

        public override Vector2 getFootprintSize() {
            return Vector2.one;
        }
    }
}
