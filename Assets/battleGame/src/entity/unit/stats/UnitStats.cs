using fNbt;
using src.util;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace src.entity.unit.stats {

    public class UnitStats {

        public bool dirty;

        private string firstName;
        private string lastName;
        private Gender gender;

        public readonly StatFloat distanceWalked;
        public readonly StatTime timeAlive;
        public readonly StatInt unitsKilled;
        public readonly StatInt buildingsDestroyed;
        public readonly StatInt damageDelt;
        public readonly StatInt damageTaken;

        // Builder specific
        public readonly StatInt resourcesCollected;
        public readonly StatInt buildingsBuilt;

        // Not yet implemented, they are never increased.
        public readonly StatInt buildingUpgraded;
        public readonly StatInt repairsDone;

        public UnitStats() {
            this.distanceWalked = new StatFloat(this, "Distance Walked", "disWalked");
            this.timeAlive = new StatTime(this, "Time Alive", "timeAlive");
            this.unitsKilled = new StatInt(this, "Units Killed", "uKills");
            this.buildingsDestroyed = new StatInt(this, "Buildings Destroyed", "buildingsDestoryed");
            this.damageDelt = new StatInt(this, "Damage Delt", "damageDelt");
            this.damageTaken = new StatInt(this, "Damage Taken", "damageTaken");
            this.resourcesCollected = new StatInt(this, "Resources Collected", "resCollected");
            this.buildingsBuilt = new StatInt(this, "Buildings Built", "buildingsBuilt");
            this.buildingUpgraded = new StatInt(this, "Building Upgrades", "buildingUpgrades");
            this.repairsDone = new StatInt(this, "Repairs Done", "repairsDone");

            this.gender = Random.Range(0, 1) == 0 ? Gender.MALE : Gender.FEMALE;
            Names.getRandomName(this.gender, out this.firstName, out this.lastName);
        }

        /// <summary>
        /// Returns the full name, first and last, of the unit.
        /// </summary>
        public string getName() {
            return this.firstName + " " + this.lastName;
        }

        public Gender getGender() {
            return this.gender;
        }

        public void readFromNbt(NbtCompound tag) {
            this.firstName = tag.getString("firstName");
            this.lastName = tag.getString("lastName");
            this.gender = tag.getByte("gender") == 1 ? Gender.MALE : Gender.FEMALE;

            this.distanceWalked.read(tag);
            this.timeAlive.read(tag);
            this.unitsKilled.read(tag);
            this.buildingsDestroyed.read(tag);
            this.damageDelt.read(tag);
            this.damageTaken.read(tag);
        }

        public NbtCompound writeToNBT(NbtCompound tag) {
            tag.setTag("firstName", this.firstName);
            tag.setTag("lastName", this.lastName);
            tag.setTag("gender", this.gender == Gender.MALE ? 1 : 2);

            this.distanceWalked.write(tag);
            this.timeAlive.write(tag);
            this.unitsKilled.write(tag);
            this.buildingsDestroyed.write(tag);
            this.damageDelt.write(tag);
            this.damageTaken.write(tag);

            return tag;
        }

        public string getFormattedStatString(bool isBuilder) {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.distanceWalked.ToString());
            sb.AppendLine(this.timeAlive.ToString());
            sb.AppendLine(this.unitsKilled.ToString());
            sb.AppendLine(this.buildingsDestroyed.ToString());
            sb.AppendLine(this.damageDelt.ToString());
            sb.AppendLine(this.damageTaken.ToString());

            if(isBuilder) {
                sb.AppendLine(this.resourcesCollected.ToString());
                sb.AppendLine(this.buildingsBuilt.ToString());
            }
            return sb.ToString();
        }
    }
}
