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
        // Health
        public const int HEALTH_BUILDER = 35;
        public const int HEALTH_SOLDIER = 75;
        public const int HEALTH_ARCHER = 50;
        public const int HEALTH_HEAVY = 100;
        // Builder
        public const int BUILDER_MAX_CARRY = 500;
        public const int BUILDER_COLLECT_PER_STRIKE = 25;

        public const float TROOP_ATTACK_RATE = 1; // Seconds between attacks.
        #endregion

        public const int DAMAGE_BUILDER = 5;
        public const int DAMAGE_SOLDIER = 5;
        public const int DAMAGE_ARCHER = 5; // Bow damage
        public const int DAMAGE_HEAVY = 5;

        #region Buildings:
        public static readonly BuildingData BD_CAMP = new BuildingData(250, 100);
        public static readonly BuildingData BD_WORKSHOP = new BuildingData(250, 350);
        public static readonly BuildingData BD_TRAINING_HOUSE = new BuildingData(250, 200);
        public static readonly BuildingData BD_STOREROOM = new BuildingData(250, 250);
        public static readonly BuildingData BD_TOWER = new BuildingData(250, 250);
        public static readonly BuildingData BD_WALL = new BuildingData(500, 150);

        public const int CONSTRUCT_RATE = 10;
        #endregion
    }
}
