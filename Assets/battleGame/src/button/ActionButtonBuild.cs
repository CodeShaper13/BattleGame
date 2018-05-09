using src.buildings;
using src.data;
using src.registry;
using src.entity.unit;

namespace src.button {

    public class ActionButtonBuild : ActionButton {

        private readonly string buttonText;

        public ActionButtonBuild(string actionName, RegisteredObject obj) : base(actionName, -1) {
            this.function = (unit) => {
                BuildOutline.instance().enableOutline(obj, (UnitBuilder)unit);
            };

            BuildingData data = obj.getPrefab().GetComponent<BuildingBase>().getData();
            this.buttonText = data.getName() + " (" + data.getCost() + ")";
        }

        public override string getText() {
            return buttonText;
        }
    }
}
