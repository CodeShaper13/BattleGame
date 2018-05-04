using src.buildings;
using src.troop;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using src.data;

namespace src {

    public class CameraMover : SidedObjectBase {

        public static CameraMover singleton;

        // Object references;
        public Text resourceText;
        public Text troopCountText;
        public BuildOutline buildOutline;

        public SelectedParty party;
        public ActionButtons actionButtons;

        // Options
        public float sensitivity = 0.4f;

        private int resources;

        [HideInInspector]
        public BuildingBase selectedBuilding;

        protected override void onAwake() {
            CameraMover.singleton = this;

            base.onAwake();
        }

        protected override void onStart() {
            base.onStart();

            this.party.setCameraMover(this);

            this.setResources(Constants.STARTING_RESOURCES);
        }

        protected override void onUpdate() {
            base.onUpdate();

            float forwardSpeed = Input.GetAxis("Horizontal") * this.sensitivity;
            float sideSpeed = Input.GetAxis("Vertical") * this.sensitivity;

            this.transform.transform.position += new Vector3(forwardSpeed, 0, sideSpeed);

            // Clicking.
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && BuildOutline.isDisabled()) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000)) {
                    SidedObjectEntity soe = hit.transform.gameObject.GetComponent<SidedObjectEntity>();
                    if (soe != null && soe.getTeam() == this.getTeam()) {

                        if(soe is UnitBase) {
                            bool flag = this.party.tryAdd((UnitBase)soe);
                        } else if(soe is BuildingBase) {
                            this.party.disband();
                            this.selectedBuilding = (BuildingBase)soe;
                        } else {
                            print("Error?  " + soe.gameObject.name);
                        }

                        //soe.onClick(this);

                        this.actionButtons.updateButtons();
                    } else if(hit.transform.name == "Ground") {
                        this.party.moveAllTo(hit.point);
                    }
                }
            }

            this.updateCount();
        }

        /// <summary>
        /// Centers the camera on the passed position.
        /// </summary>
        public void centerOn(Vector3 pos) {
            Vector3 v = pos - (this.transform.forward * 10);
            this.transform.position = new Vector3(v.x, this.transform.position.y, v.z);
        }

        /// <summary>
        /// Updates the troop and player count.
        /// </summary>
        public void updateCount() {
            int currentTroopCount = 0;
            int maxResources = this.getMaxResourceCount();
            foreach (SidedObjectBase o in this.getTeam().getMembers()) {
                if (o is UnitBase) {
                    currentTroopCount++;
                }
            }

            int max = this.getMaxTroopCount();
            this.troopCountText.text = "Troop " + currentTroopCount + "/" + max;
            this.troopCountText.fontStyle = (currentTroopCount == max ? FontStyle.Bold : FontStyle.Normal);

            this.resourceText.text = "Resources: " + this.resources + "/" + maxResources;
            this.resourceText.fontStyle = (this.resources == maxResources ? FontStyle.Bold : FontStyle.Normal);
        }

        /// <summary>
        /// Returns the current number of resources that this team has.
        /// </summary>
        public int getResources() {
            return this.resources;
        }

        /// <summary>
        /// Sets the players resources, clamping it between 0 and the maximum number the player can have.  Any overflow is discarded.
        /// </summary>
        public void setResources(int amount) {
            this.resources = Mathf.Clamp(this.resources + amount, 0, this.getMaxResourceCount());
        }

        /// <summary>
        /// Returns the maximum amount of resources that this player can have.
        /// </summary>
        public int getMaxResourceCount() {
            int maxResources = Constants.DEFAUT_RESOURCE_CAP;
            foreach (SidedObjectBase o in this.getTeam().getMembers()) {
                if (o is BuildingStoreroom) {
                    maxResources += Constants.STOREROOM_RESOURCE_BOOST;
                }
            }
            return maxResources;
        }

        /// <summary>
        /// Returns the total number of troops this player can have.
        /// </summary>
        public int getMaxTroopCount() {
            int i = Constants.DEFAULT_TROOP_CAP;
            foreach(SidedObjectBase o in this.getTeam().getMembers()) {
                if(o is BuildingCamp) {
                    i += Constants.CAMP_TROOP_BOOST;
                }
            }
            return i;
        }
    }
}
