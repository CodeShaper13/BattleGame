namespace src.registry {

    public class EntityRegistry : RegistryBase {

        public static RegisteredObject unitBuilder;
        public static RegisteredObject unitSoldier;
        public static RegisteredObject unitArcher;
        public static RegisteredObject unitHeavy;

        protected override void initRegistry() {
            EntityRegistry.unitBuilder = register(0, References.list.unitBuilder);
            EntityRegistry.unitSoldier = register(1, References.list.unitSoldier);
            EntityRegistry.unitArcher = register(2, References.list.unitArcher);
            EntityRegistry.unitHeavy = register(3, References.list.unitHeavy);
        }
    }
}
