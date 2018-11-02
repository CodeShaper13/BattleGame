using codeshaper.buildings;
using codeshaper.data;
using codeshaper.registry;
using codeshaper.entity.unit;

namespace codeshaper.button {

    public class ActionButtonBuild : ActionButtonChild {

        private readonly string buttonText;
        private BuildingData buildingData;

        public ActionButtonBuild(string actionName, RegisteredObject obj) : base(actionName) {
            this.setMainActionFunction((unit) => {
                CameraMover.instance().buildOutline.enableOutline(obj, (UnitBuilder)unit);
            });
            this.buildingData = obj.getPrefab().GetComponent<BuildingBase>().getData();
            this.buttonText = this.buildingData.getName() + " (" + this.buildingData.getCost() + ")";

            this.setShouldDisableFunction((entity) => {
                int cost = this.buildingData.getCost();
                return cost > entity.getTeam().getResources();
            });
        }

        public override string getText() {
            return buttonText;
        }
    }
}
