namespace src.registry {

    public class BuildingRegistry : Registry {

        public static RegisteredObject buildingCamp;
        public static RegisteredObject buildingWorkshop;
        public static RegisteredObject buildingTrainingHouse;
        public static RegisteredObject buildingStoreroom;
        public static RegisteredObject buildingTower;
        public static RegisteredObject buldingWall;

        protected override Registry initRegistry() {
            BuildingRegistry.buildingCamp = register(0, References.list.buildingCamp);
            BuildingRegistry.buildingWorkshop = register(1, References.list.buildingWorkshop);
            BuildingRegistry.buildingTrainingHouse = register(2, References.list.buildingTrainingHouse);
            BuildingRegistry.buildingStoreroom = register(3, References.list.buildingStoreroom);
            BuildingRegistry.buildingTower = register(4, References.list.buildingTower);
            BuildingRegistry.buldingWall = register(5, References.list.buldingWall);

            return this;
        }
    }
}
