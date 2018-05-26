using src.entity.unit.stats;
using src.registry;
using UnityEngine;

namespace src {

    /// <summary>
    /// Holds references to all of the used prefabs.
    /// </summary>
    public class References : MonoBehaviour {

        /// <summary> Static singleton of the References object. </summary>
        public static References list;

        public TextAsset maleNames;
        public TextAsset femaleNames;
        public TextAsset lastNames;

        // Units
        public GameObject unitBuilder;
        public GameObject unitSoldier;
        public GameObject unitArcher;
        public GameObject unitHeavy;

        // Buildings
        public GameObject buildingCamp;
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
