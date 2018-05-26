using src.buildings;
using src.entity.unit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using src.data;
using src.entity;
using src.party;

namespace src {

    public class CameraMover : SidedObjectBase {

        private static CameraMover singleton;

        private bool isPaused;

        // Object references;
        public Text resourceText;
        public Text troopCountText;
        public BuildOutline buildOutline;

        public SelectedParty party;
        public ActionButtons actionButtons;

        // Options
        public float sensitivity = 0.4f;

        //[HideInInspector]
        public BuildingBase selectedBuilding;

        public static CameraMover instance() {
            return CameraMover.singleton;
        }

        protected override void onAwake() {
            CameraMover.singleton = this;

            base.onAwake();
        }

        protected override void onStart() {
            base.onStart();

            this.party.setCameraMover(this);
        }

        protected override void onUpdate() {
            base.onUpdate();

            Main main = Main.instance();

            if(main.isPaused()) {
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    main.resumeGame();
                }
            } else {
                // Not paused.
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    if (BuildOutline.instance().isDisabled()) {
                        this.actionButtons.closePopupButtons();
                        main.pauseGame();
                    } else {
                        BuildOutline.instance().disableOutline();
                    }
                } else {
                    float forwardSpeed = Input.GetAxis("Horizontal") * this.sensitivity;
                    float sideSpeed = Input.GetAxis("Vertical") * this.sensitivity;

                    this.transform.transform.position += new Vector3(forwardSpeed, 0, sideSpeed);

                    if(BuildOutline.instance().isDisabled()) {
                        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) {
                                SidedObjectEntity entity = hit.transform.gameObject.GetComponent<SidedObjectEntity>();
                                if (entity != null && entity.getTeam() == this.getTeam()) {
                                    if (entity is UnitBase) {
                                        this.selectedBuilding = null;
                                        bool flag = this.party.tryAdd((UnitBase)entity);
                                    }
                                    else if (entity is BuildingBase) {
                                        this.party.disband();
                                        this.selectedBuilding = (BuildingBase)entity;
                                    }
                                    else {
                                        print("Error?  " + entity.gameObject.name);
                                    }

                                    //soe.onClick(this);

                                    this.actionButtons.updateButtons();
                                }
                                else if (hit.transform.name == "Ground") {
                                    this.actionButtons.closePopupButtons();
                                    this.party.moveAllTo(hit.point);
                                }
                            }
                        }
                    } else {
                        // Build outline specific input.
                        BuildOutline.instance().updateOutline();
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
            int maxResources = this.getTeam().getMaxResourceCount();
            foreach (SidedObjectBase o in this.getTeam().getMembers()) {
                if (o is UnitBase) {
                    currentTroopCount++;
                }
            }

            int max = this.getMaxTroopCount();
            this.troopCountText.text = "Troop " + currentTroopCount + "/" + max;
            this.troopCountText.fontStyle = (currentTroopCount == max ? FontStyle.Bold : FontStyle.Normal);

            int res = this.getTeam().getResources();
            this.resourceText.text = "Resources: " + res + "/" + maxResources;
            this.resourceText.fontStyle = (res == maxResources ? FontStyle.Bold : FontStyle.Normal);
        }

        /// <summary>
        /// Returns the total number of troops this player can have.
        /// </summary>
        public int getMaxTroopCount() {
            int i = Constants.DEFAULT_TROOP_CAP;
            foreach(SidedObjectBase o in this.getTeam().getMembers()) {
                if(o is BuildingCamp) {
                    BuildingCamp camp = (BuildingCamp)o;
                    if(!camp.isConstructing) {
                        i += Constants.CAMP_TROOP_BOOST;
                    }
                }
            }
            return i;
        }
    }
}
