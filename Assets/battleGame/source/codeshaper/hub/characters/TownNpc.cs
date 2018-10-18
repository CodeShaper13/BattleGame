using UnityEngine;
using UnityEngine.UI;

namespace codeshaper.hub.characters {

    public class TownNpc : MonoBehaviour {

        [Header("The text that the NPC will display.")]
        public string[] messages;
        private bool outofRangeLastTick;

        private Text textComponent;

        private void Awake() {
            this.textComponent = this.GetComponentInChildren<Text>();
        }

        private void Update() {
            TownPlayer player = TownPlayer.instance();

            Vector2 v1 = new Vector2(this.transform.position.x, this.transform.position.z);
            Vector2 v2 = new Vector2(player.transform.position.x, player.transform.position.z);
            if(Vector2.Distance(v1, v2) < 4f) {
                // Close enough to display message.
                if(string.IsNullOrEmpty(this.textComponent.text)) {
                    this.textComponent.text = this.getMessage();
                }

                Vector3 dir = player.transform.position - transform.position;
                dir.y = 0;
                Quaternion rot = Quaternion.LookRotation(dir);
                this.transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5 * Time.deltaTime);
            } else {
                this.textComponent.text = string.Empty;
            }
        }

        private string getMessage() {
            if(this.messages.Length == 0) {
                return "Hello World!";
            } else {
                return this.messages[Random.Range(0, messages.Length)];
            }
        }
    }
}
