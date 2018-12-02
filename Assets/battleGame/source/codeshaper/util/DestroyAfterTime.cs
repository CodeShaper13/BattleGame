using UnityEngine;

namespace codeshaper.util {

    public class DestroyAfterTime : MonoBehaviour {

        public MonoBehaviour script;
        public GameObject gameObj;

        public float time;

        private void Update() {
            this.time -= Time.deltaTime;

            if(this.time <= 0) {
                GameObject.Destroy(this.script);
                GameObject.Destroy(this.gameObj);
            }
        }
    }
}
