using codeshaper.buildings;
using codeshaper.data;
using codeshaper.entity.unit;
using codeshaper.registry;

namespace codeshaper.button {

    public class ActionButtonTrain : ActionButtonChild {

        private readonly string buttonText;
        private readonly EntityData entityData;

        public ActionButtonTrain(RegisteredObject obj) : base(string.Empty) {
            this.entityData = obj.getPrefab().GetComponent<UnitBase>().getData();
            this.buttonText = this.entityData.getName() + " (" + this.entityData.getCost() + ")";

            this.setMainActionFunction((unit) => {
                BuildingTrainingHouse trainingHouse = (BuildingTrainingHouse)unit;
                if (trainingHouse.tryAddToQueue(obj)) {
                    // Remove resources
                    trainingHouse.getTeam().reduceResources(this.entityData.getCost());
                }
            });

            this.setShouldDisableFunction((entity) => {
                return this.entityData.getCost() > entity.getTeam().getResources();
            });
        }

        public override string getText() {
            return buttonText;
        }
    }
}
