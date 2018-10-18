namespace codeshaper.data {

    public struct EntityData {

        private readonly string name;
        private readonly int health;
        private readonly int cost;
        private readonly int damageDelt;
        private readonly float productionTime;

        public EntityData(string name, int health, int cost, int damageDelt, float productionTime) {
            this.name = name;
            this.health = health;
            this.cost = cost;
            this.damageDelt = damageDelt;
            this.productionTime = productionTime;
        }

        public string getName() {
            return this.name;
        }

        public int getHealth() {
            return this.health;
        }

        public int getCost() {
            return this.cost;
        }

        public int getDamageDelt() {
            return this.damageDelt;
        }

        public float getProductionTime() {
            return this.productionTime;
        }
    }
}
