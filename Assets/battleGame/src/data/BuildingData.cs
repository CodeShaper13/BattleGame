namespace src.data {

    public struct BuildingData {

        private readonly string name;
        private readonly int health;
        private readonly int cost;

        public BuildingData(string buildingName, int maxHealth, int cost) {
            this.name = buildingName;
            this.health = maxHealth;
            this.cost = cost;
        }

        public string getName() {
            return this.name;
        }

        public int getMaxHealth() {
            return this.health;
        }

        public int getCost() {
            return this.cost;
        }
    }
}
