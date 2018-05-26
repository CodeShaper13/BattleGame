using src.buildings;
using src.data;
using src.entity.unit;
using src.registry;

namespace src.button {

    public class ActionButtonTrain : ActionButton {

        private readonly string buttonText;

        public ActionButtonTrain(RegisteredObject obj) : base(string.Empty, -1) {
            EntityData data = obj.getPrefab().GetComponent<UnitBase>().getData();
            this.buttonText = data.getName() + " (" + data.getCost() + ")";

            this.function = (unit) => {
                BuildingTrainingHouse trainingHouse = (BuildingTrainingHouse)unit;
                bool added = trainingHouse.addToQueue(obj);

                if(added) {
                    // Remove resources
                    trainingHouse.getTeam().reduceResources(data.getCost());
                }
            };
        }

        public override string getText() {
            return buttonText;
        }
    }
}
