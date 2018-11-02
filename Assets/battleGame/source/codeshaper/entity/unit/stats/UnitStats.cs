using fNbt;
using codeshaper.util;
using System.Text;
using UnityEngine;

namespace codeshaper.entity.unit.stats {

    public class UnitStats {

        // TODO what's this for?
        public bool dirty;

        private string firstName;
        private string lastName;
        private EnumGender gender;
        private Characteristic characteristic;

        public readonly StatFloat distanceWalked;
        public readonly StatTime timeAlive;
        public readonly StatInt unitsKilled;
        public readonly StatInt buildingsDestroyed;
        public readonly StatInt damageDelt;
        public readonly StatInt damageTaken;

        // Builder specific
        public readonly StatInt resourcesCollected;
        public readonly StatInt buildingsBuilt;
        public readonly StatInt repairsDone;

        // Not yet implemented, they are never increased.
        public readonly StatInt buildingUpgraded;

        public UnitStats() {
            int easterEggRnd = Random.Range(0, 1000000);
            if (easterEggRnd == 111599) {
                this.firstName = "PJ";
                this.lastName = "Didelot";
                this.gender = EnumGender.MALE;
                this.characteristic = Characteristic.a; // TODO Set to the right thing.
            } else {
                Names.getRandomName(this.gender, out this.firstName, out this.lastName);
                this.gender = Random.Range(0, 1) == 0 ? EnumGender.MALE : EnumGender.FEMALE;
                this.characteristic = Characteristic.ALL[Random.Range(0, Characteristic.ALL.Length)];
            }

            this.distanceWalked = new StatFloat(this, "Distance Walked", "disWalked");
            this.timeAlive = new StatTime(this, "Time Alive", "timeAlive");
            this.unitsKilled = new StatInt(this, "Units Killed", "uKills");
            this.buildingsDestroyed = new StatInt(this, "Buildings Destroyed", "buildingsDestoryed");
            this.damageDelt = new StatInt(this, "Damage Delt", "damageDelt");
            this.damageTaken = new StatInt(this, "Damage Taken", "damageTaken");
            this.resourcesCollected = new StatInt(this, "Resources Collected", "resCollected");
            this.buildingsBuilt = new StatInt(this, "Buildings Built", "buildingsBuilt");
            this.repairsDone = new StatInt(this, "Repairs Done", "repairsDone");

            this.buildingUpgraded = new StatInt(this, "Building Upgrades", "buildingUpgrades");
        }

        /// <summary>
        /// Returns the full name, first and last, of the unit.
        /// </summary>
        public string getName() {
            return this.firstName + " " + this.lastName;
        }

        public EnumGender getGender() {
            return this.gender;
        }

        public void readFromNbt(NbtCompound tag) {
            this.firstName = tag.getString("firstName");
            this.lastName = tag.getString("lastName");
            this.gender = tag.getByte("gender") == 1 ? EnumGender.MALE : EnumGender.FEMALE;
            this.characteristic = Characteristic.ALL[tag.getInt("characteristicID")];

            this.distanceWalked.read(tag);
            this.timeAlive.read(tag);
            this.unitsKilled.read(tag);
            this.buildingsDestroyed.read(tag);
            this.damageDelt.read(tag);
            this.damageTaken.read(tag);
            this.resourcesCollected.read(tag);
            this.buildingsBuilt.read(tag);
            this.repairsDone.read(tag);
        }

        public NbtCompound writeToNBT(NbtCompound tag) {
            tag.setTag("firstName", this.firstName);
            tag.setTag("lastName", this.lastName);
            tag.setTag("gender", this.gender == EnumGender.MALE ? 1 : 2);
            tag.setTag("characteristicID", this.characteristic.getId());

            this.distanceWalked.write(tag);
            this.timeAlive.write(tag);
            this.unitsKilled.write(tag);
            this.buildingsDestroyed.write(tag);
            this.damageDelt.write(tag);
            this.damageTaken.write(tag);
            this.resourcesCollected.write(tag);
            this.buildingsBuilt.read(tag);
            this.repairsDone.write(tag);

            return tag;
        }

        public string getFormattedStatString(bool isBuilder) {
            StringBuilder sb = new StringBuilder();

            //sb.AppendLine((System.Math.Truncate(this.distanceWalked.get() * 100) / 100) + " km");
            sb.AppendLine(this.distanceWalked.ToString() + " km");
            sb.AppendLine(this.timeAlive.ToString());
            sb.AppendLine(this.unitsKilled.ToString());
            sb.AppendLine(this.buildingsDestroyed.ToString());
            sb.AppendLine(this.damageDelt.ToString());
            sb.AppendLine(this.damageTaken.ToString());

            if(isBuilder) {
                sb.AppendLine(this.resourcesCollected.ToString());
                sb.AppendLine(this.buildingsBuilt.ToString());
                sb.AppendLine(this.repairsDone.ToString());
            }
            return sb.ToString();
        }
    }
}
