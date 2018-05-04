using UnityEngine;

namespace src.ui {

    public class Billboard : MonoBehaviour {

        private void LateUpdate() {
            Transform t = Camera.main.transform;
            this.transform.LookAt(t);
            //this.transform.rotation = Quaternion.identity;// Quaternion.Euler(this.transform.eulerAngles.x, 90, this.transform.eulerAngles.z);
        }
    }
}
