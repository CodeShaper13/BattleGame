using codeshaper.gui;
using UnityEngine;

namespace codeshaper {

    /// <summary>
    /// Holds references to all of the used prefabs.
    /// </summary>
    public class References : MonoBehaviour {

        /// <summary> Static singleton of the References object. </summary>
        public static References list;

        public GameObject buildOutlinePrefab;

        // Text assets.
        public TextAsset maleNames;
        public TextAsset femaleNames;
        public TextAsset lastNames;
        public TextAsset constants;
        public TextAsset textCredits;

        // Entity Prefabs.
        public GameObject unitBuilder;
        public GameObject unitSoldier;
        public GameObject unitArcher;
        public GameObject unitHeavy;

        public GameObject specialWarWagon;
        public GameObject specialCannon;

        // Projectile Prefabs.
        public GameObject projectileArrow;

        // Harvestable Prefabs.
        public GameObject harvestableTreePrefab;
        public GameObject harvestableDeadTreePrefab;
        public GameObject harvestableCactusPrefab;
        public GameObject harvestableRockPrefab;
        public GameObject harvestableSkullPrefab;

        // Building Prefabs.
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
        public GuiBase guiCreditsObject;
        public GuiBase guiPausedObject;
        public GuiBase guiUnitStatsObject;
        public GuiBase guiLevelFailObject;
        public GuiBase guiLevelWinObject;
        public GuiBase guiFindUnlockableObject;
        public GuiBase guiTitleScreenObject;

        public GameObject chunkPrefab;
    }
}
