using UnityEngine;

namespace src.entity.unit.stats {

    public class Names {

        private static Names mNames;
        private static Names fNames;
        private static Names lNames;

        private readonly TextAsset textAsset;
        private string[] names;

        public static void bootstrap() {
            mNames = new Names(References.list.maleNames);
            fNames = new Names(References.list.femaleNames);
            lNames = new Names(References.list.lastNames);
        }

        public Names(TextAsset text) {
            this.textAsset = text;
            this.names = this.textAsset.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Returns a random name for the list.
        /// </summary>
        /// <returns></returns>
        public string getRndName() {
            return this.names[Random.Range(0, this.names.Length)];
        }

        public static void getRandomName(Gender gender, out string firstName, out string lastName) {
            if(gender == Gender.MALE) {
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
