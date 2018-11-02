using codeshaper.buildings;
using codeshaper.entity.unit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using codeshaper.entity;
using codeshaper.selected;
using codeshaper.team;
using codeshaper.util;
using codeshaper.button;

namespace codeshaper {

    public class CameraMover : MonoBehaviour {

        private static CameraMover singleton;

        private Main main;

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
        public ActionButtonManager actionButtons;

        //TODO not yet implemented
        /// <summary> Seconds that the left moust button has been down. </summary>
        public float leftBtnSecondsDown;
        public Vector2 rectStart;

        // Options
        public float panSensitivity = 0.4f;
        public float zoomSensitivity = 500f;

        /// <summary>
        /// Returns the instance of the camera mover that this game is using.
        /// </summary>
        public static CameraMover instance() {
            return CameraMover.singleton;
        }

        private void Awake() {
            CameraMover.singleton = this;
            this.main = Main.instance();

            this.buildOutline = GameObject.Instantiate(References.list.buildOutlinePrefab).GetComponent<BuildOutline>();

            this.team = Team.teamFromEnum(this.objectTeam);
            this.party.setCameraMover(this);
        }

        private void LateUpdate() {
            // Debug keys.
            if(Input.GetKeyDown(KeyCode.F3)) {
                Main.DEBUG = !Main.DEBUG;
                Debug.Log("Toggling Debug Mode.  It is now set to " + Main.DEBUG);
            }

            if(this.main.isPaused()) {
                // Paused.
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    this.main.resumeGame();
                }
            } else {
                // Not paused.
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    // Escape was pressed, canceling things or if nothing can be canceled, pause the game.
                    if (this.actionButtons.delayedButtonRef != null) {
                        this.actionButtons.cancelDelayedAction();
                    }
                    else if(this.buildOutline.isEnabled()) {
                        this.buildOutline.disableOutline();
                    }
                    else {
                        this.actionButtons.closePopupButtons();
                        this.main.pauseGame();
                    }
                } else {
                    this.handleCameraMovement();

                    bool leftBtnDown = Input.GetMouseButtonUp(0);
                    bool rightBtnDown = Input.GetMouseButtonUp(1);

                    if (leftBtnDown || rightBtnDown) {
                        if (!EventSystem.current.IsPointerOverGameObject()) {
                            // Over the playing field
                            if(this.buildOutline.isEnabled()) {
                                this.buildOutline.handleClick(leftBtnDown, rightBtnDown);
                            } else {
                                // See this for drawing the select rect:
                                // https://answers.unity.com/questions/720447/if-game-object-is-in-cameras-field-of-view.html

                                RaycastHit hit;
                                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity)) {
                                    SidedObjectEntity entity = hit.transform.gameObject.GetComponent<SidedObjectEntity>();

                                    if (entity == null) {
                                        if (leftBtnDown && hit.transform.CompareTag(Tags.ground)) {
                                            this.actionButtons.closePopupButtons();
                                            this.party.moveAllTo(hit.point);
                                        }
                                    }
                                    else {
                                        if (this.actionButtons.delayedButtonRef != null) {
                                            ActionButtonRequireClick delayedButton = this.actionButtons.delayedButtonRef;
                                            // Check if this is a valid option to preform the action on.
                                            if (delayedButton.isValidForAction(this.team, entity)) {
                                                if (this.selectedBuilding.getBuilding() != null) {
                                                    delayedButton.callFunction(this.selectedBuilding.getBuilding(), entity);
                                                }
                                                else {
                                                    delayedButton.callFunction(this.party.getAllUnits(), entity);
                                                }
                                            }
                                        }
                                        else {
                                            if (entity.getTeam() == this.team) {
                                                // Clicked something on our team.
                                                if (leftBtnDown) {
                                                    this.onLeftBtnClick(entity);
                                                }
                                                if (rightBtnDown) {
                                                    this.onRightBtnClick(entity);
                                                }
                                            }
                                        }
                                    }
                                    // A click happened, so if something valid was clicked the delayed action was called.
                                    // Either way, we should cancel the action becuase it was resolved or the wrong thing was clicked.
                                    this.actionButtons.cancelDelayedAction();
                                }
                            }
                        }
                    }
                }
            }
            this.updateHudCounts();
        }

        private void onLeftBtnClick(SidedObjectEntity entity) {
            if (entity is UnitBase) {
                this.selectedBuilding.setBuilding(null);
                this.party.disband();
                this.party.tryAdd((UnitBase)entity);
            } else if (entity is BuildingBase) {
                this.party.disband();
                this.selectedBuilding.setBuilding((BuildingBase)entity);
            }
            this.actionButtons.updateSideButtons();
        }

        private void onRightBtnClick(SidedObjectEntity entity) {
            if (entity is UnitBase) {
                if (this.party.contains((UnitBase)entity)) {
                    this.party.remove((UnitBase)entity);
                } else {
                    this.selectedBuilding.setBuilding(null);
                    this.party.tryAdd((UnitBase)entity);
                }
            }
            else if (entity is BuildingBase) {
                if(entity == this.selectedBuilding) {
                    this.selectedBuilding.setBuilding(null);
                } else {
                    this.party.disband();
                    this.selectedBuilding.setBuilding((BuildingBase)entity);
                }
            }
            this.actionButtons.updateSideButtons();
        }

        /// <summary>
        /// Returns the team that controls this camera.
        /// </summary>
        public Team getTeam() {
            return this.team;
        }

        /// <summary>
        /// Centers the camera on the passed position.
        /// </summary>
        public void centerOn(Vector3 pos) {
            this.transform.parent.position = new Vector3(pos.x, this.transform.parent.position.y, pos.z);
        }

        /// <summary>
        /// Updates the troop and player count text on the corner of the screen.
        /// </summary>
        public void updateHudCounts() {
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

        /// <summary>
        /// Moves and pans the camera based on the input form the user.
        /// </summary>
        private void handleCameraMovement() {
            // Move position.
            float forwardSpeed = (Input.GetAxis("Horizontal") * this.panSensitivity * Time.deltaTime);
            float sideSpeed = (Input.GetAxis("Vertical") * this.panSensitivity * Time.deltaTime);
            this.transform.parent.Translate(forwardSpeed, 0, sideSpeed);

            // Zoom in and out.
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (zoom != 0f) {
                float f = zoom * Time.deltaTime * zoomSensitivity;
                this.transform.Translate(0, 0, f);

                float LOWEST_ZOOM = -12f;
                float HIGHEST_ZOOM = -40f;

                float localZ = this.transform.localPosition.z;
                if (localZ > LOWEST_ZOOM) {
                    this.transform.Translate(0, 0, LOWEST_ZOOM - localZ);
                }
                else if (localZ < HIGHEST_ZOOM) {
                    this.transform.Translate(0, 0, HIGHEST_ZOOM - localZ);
                }
            }
        }
    }
}
