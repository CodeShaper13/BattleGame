namespace codeshaper.data {

    /// <summary>
    /// Constants that can be easily changed to help with game balancing.
    /// </summary>
    public static class Constants {

        public static int STARTING_RESOURCES = 1000000; // 300;

        public static int DEFAULT_TROOP_CAP = 6;

        public static int DEFAUT_RESOURCE_CAP = 1000000; // 500;

        #region Units:
        public static readonly EntityData ED_BUILDER = new EntityData("Builder", 35, 25, 5, 45f);
        public static readonly EntityData ED_SOLDIER = new EntityData("Soldier", 75, 25, 5, 1f);
        public static readonly EntityData ED_ARCHER = new EntityData("Archer", 50, 25, 5, 15f);
        public static readonly EntityData ED_HEAVY = new EntityData("Heavy", 100, 25, 5, 25f);

        public static readonly EntityData ED_WAR_WAGON = new EntityData("War Wagon", 500, 100, 3, 60f);
        public static readonly EntityData ED_CANNON = new EntityData("Cannon", 150, 100, 200, 45f);

        // Builder
        public static int BUILDER_MAX_CARRY = 500;
        public static int BUILDER_COLLECT_PER_STRIKE = 25;
        public static float BUILDER_STRIKE_RATE = 1f;

        // General
        public static float TROOP_ATTACK_RATE = 1f; // Seconds between attacks.
        public static float TROOP_FIRE_RATE = 1f;

        // Archer
        public static float ARCHER_SHOOT_RANGE = 10f;

        #endregion

        #region Buildings:
        public static readonly BuildingData BD_CAMP = new BuildingData("Camp", 250, 100);
        public static readonly BuildingData BD_PRODUCER = new BuildingData("Producer", 250, 200);
        public static readonly BuildingData BD_WORKSHOP = new BuildingData("Workshop", 250, 350);
        public static readonly BuildingData BD_TRAINING_HOUSE = new BuildingData("Training House", 250, 200);
        public static readonly BuildingData BD_STOREROOM = new BuildingData("Store Room", 250, 250);
        public static readonly BuildingData BD_TOWER = new BuildingData("Tower", 250, 250);
        public static readonly BuildingData BD_WALL = new BuildingData("Wall", 500, 50, true);

        public static readonly int TRAINING_HOUSE_QUEUE_SIZE = 3;
        public static readonly int WORKSHOP_QUEUE_SIZE = 2;

        public static int CONSTRUCT_RATE = 10;

        public static int BUILDING_CAMP_TROOP_BOOST = 4;

        public static int BUILDING_STOREROOM_RESOURCE_BOOST = 250;

        public static float BUILDING_PRODUCER_RATE = 2f; // How often to produce something in seconds.
        public static int BUILDING_PRODUCER_MAX_HOLD = 100;

        public static float BUILDING_TOWER_FIRE_SPEED = 1.5f;
        public static float BUILDING_TOWER_FIRE_RANGE = 20f;
        public static int BUILDING_TOWER_DAMAGE = 15;
        public static float BUILDING_TOWER_SEE_RANGE = 10f;

        #endregion


        public static void bootstrap() {
            KeyedSettings ks = new KeyedSettings(References.list.constants);

            //STARTING_RESOURCES = ks.getInt("STARTING_RESOURCES");
        }
    }
}
