using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity.unit.stats {

    public class Names {

        private static Names mNames;
        private static Names fNames;
        private static Names lNames;

        private readonly TextAsset textAsset;
        private string[] names;

        public static void bootstrap() {
            Names.mNames = new Names(References.list.maleNames);
            Names.fNames = new Names(References.list.femaleNames);
            Names.lNames = new Names(References.list.lastNames);
        }

        public Names(TextAsset text) {
            this.textAsset = text;
            this.names = FileUtils.readTextAsset(this.textAsset).ToArray(); ;
        }

        /// <summary>
        /// Returns a random name for the list.
        /// </summary>
        public string getRndName() {
            return this.names[Random.Range(0, this.names.Length)];
        }

        public static void getRandomName(EnumGender gender, out string firstName, out string lastName) {
            if(gender == EnumGender.MALE) {
                if (Random.Range(1, 1000000) == 1) {
                    firstName = "PJ";
                    lastName = "Didelot";
                }
                firstName = Names.mNames.getRndName();

            } else {
                firstName = Names.fNames.getRndName();
            }
            lastName = Names.lNames.getRndName();
        }
    }
}
