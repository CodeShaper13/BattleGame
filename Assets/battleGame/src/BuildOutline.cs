using src.buildings;
using src.map;
using src.registry;
using src.entity.unit;
using src.util;
using UnityEngine;

namespace src {

    public class BuildOutline : MonoBehaviour {

        private const float HEIGHT = 0.1f;

        private static BuildOutline singleton;

        [SerializeField]
        private Material invalidMaterial;
        [SerializeField]
        private Material validMaterial;
        private MeshRenderer meshRenderer;

        private RegisteredObject buildingToPlace;
        private UnitBuilder builder;

        public static BuildOutline instance() {
            return BuildOutline.singleton;
        }

        private void Awake() {
            BuildOutline.singleton = this;

            this.meshRenderer = this.GetComponent<MeshRenderer>();

            // Make sure this object is at the right height.
            this.transform.position = new Vector3(this.transform.position.x, HEIGHT, this.transform.position.z);

            this.disableOutline();
        }

        private void Update() {
            // Update Color.
            if (this.isSpaceFree()) {
                this.meshRenderer.material = this.validMaterial;
            } else {
                this.meshRenderer.material = this.invalidMaterial;
            }
        }

        /// <summary>
        /// Returns true if the build outline is disabled and normal clicking can be handled.
        /// </summary>
        public bool isDisabled() {
            return !BuildOutline.singleton.gameObject.activeSelf;
        }

        public void updateOutline() {
            RaycastHit hit;
            Vector3 correctedHit = Vector3.zero;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                correctedHit = new Vector3(
                    Mathf.Round(hit.point.x),
                    HEIGHT,
                    Mathf.Round(hit.point.z));
                this.transform.position = correctedHit;
            }

            if (Input.GetMouseButtonDown(0) && this.isSpaceFree()) {
                CameraMover cm = CameraMover.instance();

                // Create the new building.                
                BuildingBase newBuilding = (BuildingBase)Map.getInstance().spawnEntity(this.buildingToPlace, new Vector3(correctedHit.x, 0, correctedHit.z), Quaternion.identity);
                newBuilding.setTeam(cm.getTeam());
                newBuilding.setHealth(1);
                newBuilding.setConstructing();

                // Remove resources.
                cm.getTeam().reduceResources(newBuilding.getData().getCost());
                this.builder.setBuilding(newBuilding);

                this.disableOutline();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                this.disableOutline();
            }
        }

        /// <summary>
        /// Called to enable the outline effect.
        /// </summary>
        public void enableOutline(RegisteredObject obj, UnitBuilder builder) {
            this.gameObject.SetActive(true);

            this.buildingToPlace = obj;
            this.builder = builder;

            this.setSize(this.buildingToPlace.getPrefab().GetComponent<BuildingBase>());
        }

        /// <summary>
        /// Disables the outline effect.
        /// </summary>
        public void disableOutline() {
            this.gameObject.SetActive(false);
        }

        public void setSize(BuildingBase building) {
            Vector2 scale = building.getFootprintSize();
            this.transform.localScale = new Vector3(scale.x - 0.1f, scale.y - 0.1f, 0.01f);
        }

        public bool isSpaceFree() {
            return !Physics.CheckBox(this.transform.position, this.transform.lossyScale / 2, this.transform.rotation, ~Layers.GEOMETRY);
        }
    }
}
