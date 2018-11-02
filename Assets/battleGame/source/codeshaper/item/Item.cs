using UnityEngine;

namespace codeshaper.item {

    public class Item {

        public static Item yoke;
        public static Item playingCard;
        public static Item hammer;
        public static Item glasses;
        public static Item key;


        private readonly string name;
        private readonly string description;
        private readonly GameObject obj;

        public Item(string itemName, string description, GameObject obj) {
            this.name = itemName;
            this.description = description;
            this.obj = obj;
        }

        public string getName() {
            return this.name;
        }

        public string getDescription() {
            return this.description;
        }

        public static void bootstrap() {
            yoke = new Item("Yoke", "A wooden yoke for oxen or caddle", null);
            playingCard = new Item("Playing Card", "description", null);
            hammer = new Item("Hammer", "description", null);
            glasses = new Item("Reading Glasses", "description", null);
            key = new Item("Key", "description", null);
        }
    }
}
