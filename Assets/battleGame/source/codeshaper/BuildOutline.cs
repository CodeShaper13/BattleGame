using codeshaper.buildings;
using codeshaper.map;
using codeshaper.registry;
using codeshaper.entity.unit;
using codeshaper.util;
using UnityEngine;
using codeshaper.data;
using System.Collections.Generic;
using codeshaper.team;
using codeshaper.entity.unit.task.builder;

namespace codeshaper {

    public class BuildOutline : MonoBehaviour {

        private const float HEIGHT = 0.1f;

        private static BuildOutline singleton;

        [SerializeField]
        private Material invalidMaterial;
        [SerializeField]
        private Material validMaterial;
        private MeshRenderer meshRenderer;
        private RegisteredObject buildingToPlace;
        private List<UnitBuilder> cachedBuilders;

        public static BuildOutline instance() {
            return BuildOutline.singleton;
        }

        private void Awake() {
            BuildOutline.singleton = this;

            this.cachedBuilders = new List<UnitBuilder>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();

            // Make sure this object is at the right height.
            this.transform.position.setY(HEIGHT);
        }

        private void Start() {
            this.disableOutline();
        }

        private void Update() {
            RaycastHit hit;
            Vector3 correctedHit = Vector3.zero;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                correctedHit = new Vector3(
                    Mathf.Round(hit.point.x),
                    HEIGHT,
                    Mathf.Round(hit.point.z));
                this.transform.position = correctedHit;
            }

            // Update Color.
            if (this.isSpaceFree()) {
                this.meshRenderer.material = this.validMaterial;
            } else {
                this.meshRenderer.material = this.invalidMaterial;
            }
        }

        /// <summary>
        /// Returns true if the build outline is enabled outline specific input should be
        /// handled instead of normal clicking.
        /// </summary>
        public bool isEnabled() {
            return this.gameObject.activeSelf;
        }

        public void handleClick() {
            if (Input.GetMouseButtonUp(0) && this.isSpaceFree()) {
                // Pick the builder to use.
                UnitBuilder builder = Util.closestToPoint(this.transform.position, this.cachedBuilders, (entity) => {
                    return entity.getTask().cancelable();
                });

                if(builder != null) {
                    Team team = builder.getTeam();

                    // Create the new building GameObject and set it's team.                
                    BuildingBase newBuilding = (BuildingBase)Map.instance().spawnEntity(this.buildingToPlace, new Vector3(this.transform.position.x, 0, this.transform.position.z), Quaternion.identity);
                    newBuilding.setTeam(team);

                    // Remove resources from the builder's team.
                    int buildingCost = newBuilding.getData().getCost();
                    team.reduceResources(buildingCost);

                    BuildingData bd = newBuilding.getData();
                    if (bd.isInstantBuild()) {
                        newBuilding.setHealth(bd.getMaxHealth());
                    }
                    else {
                        newBuilding.setHealth(1);
                        builder.setTask(new TaskConstructBuilding(builder, newBuilding));
                    }

                    if (newBuilding is BuildingWall && team.getResources() >= buildingCost) {
                        // TODO move outline over?
                    }
                    else {
                        this.disableOutline();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                this.disableOutline();
            }
        }

        /// <summary>
        /// Called to enable the outline effect.
        /// </summary>
        public void enableOutline(RegisteredObject registeredBuilding, UnitBuilder builder) {
            // Note, this could be called multiple times if multiple builders are in the same party.
            this.gameObject.SetActive(true);

            this.buildingToPlace = registeredBuilding;
            this.cachedBuilders.Add(builder);

            this.setSize(this.buildingToPlace.getPrefab().GetComponent<BuildingBase>());

            // Gray out the action buttons while the outline is being shown.
            CameraMover.instance().actionButtons.setForceDisabled(true);
        }

        /// <summary>
        /// Disables the outline effect.
        /// </summary>
        public void disableOutline() {
            this.gameObject.SetActive(false);
            this.cachedBuilders.Clear();
            CameraMover.instance().actionButtons.setForceDisabled(false);
        }

        /// <summary>
        /// Sets the size of the outline base effect.
        /// </summary>
        private void setSize(BuildingBase building) {
            Vector2 scale = building.getFootprintSize();
            this.transform.localScale = new Vector3(scale.x - 0.1f, scale.y - 0.1f, 0.01f);
        }

        private bool isSpaceFree() {
            return !Physics.CheckBox(this.transform.position, this.transform.lossyScale / 2, this.transform.rotation, ~Layers.GEOMETRY);
        }
    }
}
