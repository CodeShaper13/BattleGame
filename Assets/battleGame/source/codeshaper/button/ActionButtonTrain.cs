using codeshaper.buildings;
using codeshaper.data;
using codeshaper.entity.unit;
using codeshaper.registry;

namespace codeshaper.button {

    public class ActionButtonTrain : ActionButtonChild {

        private readonly string buttonText;
        private EntityData entityData;

        public ActionButtonTrain(RegisteredObject obj) : base(string.Empty) {
            this.entityData = obj.getPrefab().GetComponent<UnitBase>().getData();
            this.buttonText = this.entityData.getName() + " (" + this.entityData.getCost() + ")";

            this.function = (unit) => {
                BuildingTrainingHouse trainingHouse = (BuildingTrainingHouse)unit;
                bool added = trainingHouse.tryAddToQueue(obj);

                if(added) {
                    // Remove resources
                    trainingHouse.getTeam().reduceResources(this.entityData.getCost());
                }
            };
        }

        public override string getText() {
            return buttonText;
        }

        public EntityData getEntityData() {
            return this.entityData;
        }
    }
}
