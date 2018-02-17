using src.troop;
using UnityEngine;

namespace src {

    public class HarvestableObject : MonoBehaviour {

        public EnumType type;

        private Health health;

        private void Awake() {
            this.health = this.GetComponentInChildren<Health>();

            this.health.setMaxHealth(this.getTotalResourceYield());
        }

        public void harvest(UnitBuilder builder) {
            int i = Mathf.Max(Constants.BUILDER_COLLECT_PER_STRIKE, Constants.BUILDER_MAX_CARRY - builder.resources);
            //TOOD not right?
            builder.resources = i;
            if (this.health.setHealth(this.health.getHealth() - i)) {
                GameObject.Destroy(this.gameObject);
            }
        }

        private int getTotalResourceYield() {
            switch(this.type) {
                case EnumType.BUSH: return 50;
                case EnumType.TREE: return 100;
                case EnumType.BIG_TREE: return 200;
                case EnumType.ROCK: return 150;
                case EnumType.BIG_ROCK: return 300;
            }
            return 0;
        }

        public enum EnumType {
            BUSH = 0,
            TREE = 1,
            BIG_TREE = 2,
            ROCK = 3,
            BIG_ROCK = 4,
        }
    }
}
