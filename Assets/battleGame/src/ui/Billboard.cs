using UnityEngine;

namespace src.ui {

    /// <summary>
    /// Rotates the object to face the camera at the end of every frame.
    /// </summary>
    public class Billboard : MonoBehaviour {

        /// <summary> Reference to the value of Camera.main </summary>
        private Camera mainCamera;

        private void Awake() {
            this.mainCamera = Camera.main;
        }

        private void LateUpdate() {
            Vector3 worldPos = transform.position + this.mainCamera.transform.rotation * Vector3.forward;
            this.transform.LookAt(worldPos, this.mainCamera.transform.rotation * Vector3.up);
        }
    }
}
