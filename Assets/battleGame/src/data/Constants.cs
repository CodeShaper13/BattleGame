namespace src.data {

    /// <summary>
    /// Constants that can be easily changed to help with game balancing.
    /// </summary>
    public static class Constants {

        public const int STARTING_RESOURCES = 300;

        public const int DEFAULT_TROOP_CAP = 6;
        public const int CAMP_TROOP_BOOST = 4;

        public const int DEFAUT_RESOURCE_CAP = 500;
        public const int STOREROOM_RESOURCE_BOOST = 250;

        #region Units:
        public static readonly EntityData ED_BUILDER = new EntityData("Builder", 35, 25, 5);
        public static readonly EntityData ED_SOLDIER = new EntityData("Soldier", 75, 25, 5);
        public static readonly EntityData ED_ARCHER = new EntityData("Archer", 50, 25, 5);
        public static readonly EntityData ED_HEAVY = new EntityData("Heavy", 100, 25, 5);

        // Builder
        public const int BUILDER_MAX_CARRY = 500;
        public const int BUILDER_COLLECT_PER_STRIKE = 25;
        public const float BUILDER_STRIKE_RATE = 1f;

        public const float TROOP_ATTACK_RATE = 1; // Seconds between attacks.
        #endregion

        #region Buildings:
        public static readonly BuildingData BD_CAMP = new BuildingData("Camp", 250, 100);
        public static readonly BuildingData BD_WORKSHOP = new BuildingData("Workshop", 250, 350);
        public static readonly BuildingData BD_TRAINING_HOUSE = new BuildingData("Training House", 250, 200);
        public static readonly BuildingData BD_STOREROOM = new BuildingData("Store Room", 250, 250);
        public static readonly BuildingData BD_TOWER = new BuildingData("Tower", 250, 250);
        public static readonly BuildingData BD_WALL = new BuildingData("Wall", 500, 150);

        public const int CONSTRUCT_RATE = 10;

        public const int TRAINING_CAMP_QUEUE_SIZE = 5;
        public const float TIME_TO_TRAIN = 2.5f;
        #endregion
    }
}
