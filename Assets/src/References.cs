using UnityEngine;

namespace src {

    public class References : MonoBehaviour {

        public static References list;

        private void Awake() {
            References.list = this;
        }

        // Units
        public GameObject builder;
        public GameObject commander; // Not implemented
        public GameObject soldier;
        public GameObject archer;
        public GameObject knight;

        // Buildings
        public GameObject buildingFarm;
        public GameObject buildingWorkshop;
        public GameObject buildingTrainingHouse;
        public GameObject buildingTower;
        public GameObject buldingWall;
    }
}
