using src.buildings;
using src.util;
using UnityEngine;

namespace src {

    public class BuildOutline : MonoBehaviour {

        private const float HEIGHT = 0.25f;

        public static BuildOutline reference;

        [SerializeField]
        private Material invalidMaterial;
        [SerializeField]
        private Material validMaterial;
        private MeshRenderer meshRenderer;

        private GameObject buildingPrefab;

        /// <summary>
        /// Returns true if the build outline is disabled and normal clicking can be handled.
        /// </summary>
        public static bool isDisabled() {
            return !BuildOutline.reference.gameObject.activeSelf;
        }

        private void Awake() {
            BuildOutline.reference = this;

            this.meshRenderer = this.GetComponent<MeshRenderer>();

            // Make sure its the right height.
            this.transform.position = new Vector3(this.transform.position.x, HEIGHT, this.transform.position.z);

            this.disableOutline();
        }

        private void Update() {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                this.transform.position = new Vector3(hit.point.x, HEIGHT, hit.point.z);
            }

            if(Input.GetMouseButtonDown(0) && this.isSpaceFree()) {
                // Create the new building.
                BuildingBase newbuilding = GameObject.Instantiate(this.buildingPrefab, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.identity).GetComponent<BuildingBase>();
                newbuilding.setTeam(CameraMover.singleton.getTeam());
                newbuilding.setHealth(1);
                newbuilding.setConstructing();

                this.disableOutline();
            }

            // Update Color.
            if (this.isSpaceFree()) {
                this.meshRenderer.material = this.validMaterial;
            } else {
                this.meshRenderer.material = this.invalidMaterial;
            }
        }

        public void enableOutline(GameObject prefab) {
            this.gameObject.SetActive(true);

            this.buildingPrefab = prefab;

            this.setSize(this.buildingPrefab.GetComponent<BuildingBase>());
        }

        private void disableOutline() {
            this.gameObject.SetActive(false);
        }

        public void setSize(BuildingBase building) {
            Vector2 scale = building.getFootprintSize();
            this.transform.localScale = new Vector3(scale.x, scale.y, 0.01f);
        }

        public bool isSpaceFree() {
            return !Physics.CheckBox(this.transform.position, this.transform.lossyScale / 2, this.transform.rotation, ~Layers.GEOMETRY);
        }
    }
}
