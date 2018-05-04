namespace src.data {

    public struct BuildingData {

        private readonly int health;
        private readonly int cost;

        public BuildingData(int health, int cost) {
            this.health = health;
            this.cost = cost;
        }

        public int getHealth() {
            return this.health;
        }

        public int getCost() {
            return this.cost;
        }
    }
}
