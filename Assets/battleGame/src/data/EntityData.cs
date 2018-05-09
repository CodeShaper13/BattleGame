namespace src.data {

    public struct EntityData {

        private readonly string name;
        private readonly int health;
        private readonly int cost;
        private readonly int damageDelt;

        public EntityData(string name, int health, int cost, int damageDelt) {
            this.name = name;
            this.health = health;
            this.cost = cost;
            this.damageDelt = damageDelt;
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
    }
}
