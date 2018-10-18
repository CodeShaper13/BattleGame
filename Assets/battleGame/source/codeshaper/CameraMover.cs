using codeshaper.buildings;
using codeshaper.entity.unit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using codeshaper.entity;
using codeshaper.selected;
using codeshaper.team;
using codeshaper.util;

namespace codeshaper {

    public class CameraMover : MapObject {

        private static CameraMover singleton;

        private bool isPaused;

        [SerializeField]
        private EnumTeam objectTeam;
        private Team team;

        // Object references;
        public Text resourceText;
        public Text troopCountText;
        public BuildOutline buildOutline;

        public SelectedParty party;
        public SelectedBuilding selectedBuilding;
        public ActionButtons actionButtons;

        // Options
        public float panSensitivity = 0.4f;
        public float zoomSensitivity = 500f;


        public static CameraMover instance() {
            return CameraMover.singleton;
        }

        protected override void onAwake() {
            CameraMover.singleton = this;

            base.onAwake();
        }

        protected override void onStart() {
            base.onStart();

            this.team = Team.teamFromEnum(this.objectTeam);
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
                    // Move position.
                    float forwardSpeed = (Input.GetAxis("Horizontal") * this.panSensitivity * Time.deltaTime);
                    float sideSpeed = (Input.GetAxis("Vertical") * this.panSensitivity * Time.deltaTime);
                    this.transform.parent.Translate(forwardSpeed, 0, sideSpeed);

                    // Zoom in and out.
                    float zoom = Input.GetAxis("Mouse ScrollWheel");
                    if (zoom != 0f) {
                        float f = zoom * Time.deltaTime * zoomSensitivity;
                        this.transform.Translate(0, 0, f);

                        float LOWEST_ZOOM = 12f;
                        float HIGHEST_ZOOM = -20f;

                        float localZ = this.transform.localPosition.z;
                        if (localZ > LOWEST_ZOOM) {
                            this.transform.Translate(0, 0, LOWEST_ZOOM - localZ);
                        } else if(localZ < HIGHEST_ZOOM) {
                            this.transform.Translate(0, 0, HIGHEST_ZOOM - localZ);
                        }
                    }

                    if (BuildOutline.instance().isDisabled()) {
                        // Handle mouse clicks.
                        if(!EventSystem.current.IsPointerOverGameObject()) {
                            bool flag0 = Input.GetMouseButtonDown(0);
                            bool flag1 = Input.GetMouseButtonDown(1);
                            if(flag0 || flag1) {
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                RaycastHit hit;

                                if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) {
                                    SidedObjectEntity entity = hit.transform.gameObject.GetComponent<SidedObjectEntity>();
                                    if(flag0) {
                                        if (entity != null && entity.getTeam() == this.team) {
                                            if (entity is UnitBase) {
                                                this.selectedBuilding.setBuilding(null);
                                                this.party.tryAdd((UnitBase)entity);
                                            }
                                            else if (entity is BuildingBase) {
                                                this.party.disband();
                                                this.selectedBuilding.setBuilding((BuildingBase)entity);
                                            }

                                            this.actionButtons.updateSideButtons();
                                        }
                                        else if (hit.transform.CompareTag(Tags.ground)) {
                                            this.actionButtons.closePopupButtons();
                                            this.party.moveAllTo(hit.point);
                                        }
                                    }
                                    if(flag1) {
                                        if(entity is UnitBase && this.party.contains((UnitBase)entity)) {
                                            this.party.remove((UnitBase)entity);
                                        }
                                        else if(entity is BuildingBase && entity == this.selectedBuilding) {
                                            this.selectedBuilding.setBuilding(null);
                                        }
                                    }                                    
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
        /// Returns the team that controls this camera.
        /// </summary>
        public Team getControllingTeam() {
            return this.team;
        }

        /// <summary>
        /// Centers the camera on the passed position.
        /// </summary>
        //TODO make this work
        public void centerOn(Vector3 pos) {
            Vector3 v = pos - (this.transform.forward * 10);
            this.transform.position = new Vector3(v.x, this.transform.position.y, v.z);
        }

        /// <summary>
        /// Updates the troop and player count text on the corner of the screen.
        /// </summary>
        public void updateCount() {
            int currentTroopCount = 0;
            int maxResources = this.team.getMaxResourceCount();
            foreach (SidedObjectEntity o in this.team.getMembers()) {
                if (o is UnitBase) {
                    currentTroopCount++;
                }
            }

            int max = this.team.getMaxTroopCount();
            this.troopCountText.text = "Troops: " + currentTroopCount + "/" + max;
            this.troopCountText.fontStyle = (currentTroopCount == max ? FontStyle.Bold : FontStyle.Normal);

            int res = this.team.getResources();
            this.resourceText.text = "Resources: " + res + "/" + maxResources;
            this.resourceText.fontStyle = (res == maxResources ? FontStyle.Bold : FontStyle.Normal);
        }
    }
}
