using UnityEngine;

namespace codeshaper.map {

    public class RealtimeLightHider : MonoBehaviour {

        private Light lightComponent;

        private void Awake() {
            this.lightComponent = this.GetComponent<Light>();
        }

        private void Update() {
            Vector3 screenPoint = CameraMover.instance().mainCamera.WorldToViewportPoint(this.transform.position);
            // If the screen point is less than 0 or greater than 1, hide the light.
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            this.lightComponent.enabled = onScreen;
        }
    }
}
