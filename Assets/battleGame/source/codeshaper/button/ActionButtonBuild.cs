using codeshaper.buildings;
using codeshaper.data;
using codeshaper.registry;
using codeshaper.entity.unit;

namespace codeshaper.button {

    public class ActionButtonBuild : ActionButtonChild {

        private readonly string buttonText;
        private BuildingData buildingData;

        public ActionButtonBuild(string actionName, RegisteredObject obj) : base(actionName) {
            this.function = (unit) => {
                BuildOutline.instance().enableOutline(obj, (UnitBuilder)unit);
            };
            this.buildingData = obj.getPrefab().GetComponent<BuildingBase>().getData();
            this.buttonText = this.buildingData.getName() + " (" + this.buildingData.getCost() + ")";
        }

        public override string getText() {
            return buttonText;
        }

        public BuildingData getBuildingData() {
            return this.buildingData;
        }
    }
}
