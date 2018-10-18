using UnityEngine;

namespace codeshaper {

    /// <summary>
    /// Holds references to all of the used prefabs.
    /// </summary>
    public class References : MonoBehaviour {

        /// <summary> Static singleton of the References object. </summary>
        public static References list;

        public TextAsset maleNames;
        public TextAsset femaleNames;
        public TextAsset lastNames;
        public TextAsset constants;

        // Units
        public GameObject unitBuilder;
        public GameObject unitSoldier;
        public GameObject unitArcher;
        public GameObject unitHeavy;

        public GameObject specialWarWagon;
        public GameObject specialCannon;

        // Projectilese
        public GameObject projectileArrow;

        // Buildings
        public GameObject buildingCamp;
        public GameObject buildingProducer;
        public GameObject buildingWorkshop;
        public GameObject buildingTrainingHouse;
        public GameObject buildingStoreroom;
        public GameObject buildingTower;
        public GameObject buldingWall;

        public GameObject wallJoinPiece;

        public GameObject healthBarEffect;

        // GUI
        public GameObject guiPausedObject;
        public GameObject guiUnitStatsObject;
    }
}
