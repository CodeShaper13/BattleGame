using codeshaper.entity;

namespace codeshaper.buildings.harvestable {

    public class HarvestableTree : HarvestableObject {

        public override bool damage(MapObject dealer, int amount) {
            bool flag = base.damage(dealer, amount);

            return flag;
        }

        public override void onDeathCallback() {
            base.onDeathCallback();
        }
    }
}
