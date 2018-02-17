using UnityEngine;

namespace src.buildings {

    public abstract class BuildingBase : SidedObjectEntity {

        private bool isBuilding;
        private float buildHealth;

        protected override void onUpdate() {
            base.onUpdate();

            if(this.isBuilding) {
                this.buildHealth += (Constants.BUILD_RATE * Time.deltaTime);
                if(this.buildHealth > 1) {
                    this.setHealth(this.getHealth() + 1);
                    this.buildHealth -= 1;
                }

                if(this.getHealth() >= this.getMaxHealth()) {
                    this.isBuilding = false;
                    this.buildHealth = 0;
                }
            } else {
                this.preformTask();
            }
        }

        public void setBuilding() {
            this.isBuilding = true;
        }

        public virtual void preformTask() { }

        public abstract Vector2 getFootprintSize();
    }
}
