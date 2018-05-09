using fNbt;

namespace src.entity.unit.statistics {

    public class Stats {

        public readonly StatFloat distanceWalked = new StatFloat("Distance Walked", "disWalked");
        public readonly StatFloat timeAlive = new StatFloat("Time Alive", "timeAlive");
        public readonly StatInt unitsKilled = new StatInt("Units Killed", "uKills");
        public readonly StatInt buildingsDestroyed = new StatInt("Buildings Destroyed", "buildingsDestoryed");
        public readonly StatInt damageDelt = new StatInt("Damage Delt", "damageDelt");
        public readonly StatInt damageTaken = new StatInt("Damage Taken", "damageTaken");

        // Builder specific
        public readonly StatInt resourcesCollected = new StatInt("Resources Collected", "resCollected");
        public readonly StatInt buildingsBuilt = new StatInt("Buildings Built", "buildingsBuilt");
        public readonly StatInt buildingUpgraded = new StatInt("Building Upgrads", "buildingUpgrades");
        public readonly StatInt repairsDone = new StatInt("Repairs Done", "repairsDone");

        public void readFromNbt(NbtCompound tag) {
            this.distanceWalked.read(tag);
            this.timeAlive.read(tag);
            this.unitsKilled.read(tag);
            this.buildingsDestroyed.read(tag);
            this.damageDelt.read(tag);
            this.damageTaken.read(tag);
        }

        public NbtCompound writeToNBT(NbtCompound tag) {
            this.distanceWalked.write(tag);
            this.timeAlive.write(tag);
            this.unitsKilled.write(tag);
            this.buildingsDestroyed.write(tag);
            this.damageDelt.write(tag);
            this.damageTaken.write(tag);

            return tag;
        }
    }
}
