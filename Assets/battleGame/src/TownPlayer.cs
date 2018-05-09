using UnityEngine;

namespace src {

    public class TownPlayer : MonoBehaviour {

        private static TownPlayer singleton;

        private CharacterController characterController;

        public static TownPlayer instance() {
            return TownPlayer.singleton;
        }

        private void Awake() {
            TownPlayer.singleton = this;

            this.characterController = this.GetComponent<CharacterController>();
        }

        private void Update() {
        
            // WASD Movement.
            float f = this.getMoveSpeed();
            float forwardSpeed = Input.GetAxis("Vertical") * f;
            float sideSpeed = Input.GetAxis("Horizontal") * 40f;

            // Move the Player.
            Vector3 speed = new Vector3(0, 0, forwardSpeed);
            speed = this.transform.rotation * speed;

            this.characterController.Move(speed * Time.deltaTime);
            this.gameObject.transform.Rotate(0, sideSpeed * Time.deltaTime, 0);

            // Footstep sounds, do later.
            /*
            if (this.footstepTimer > 0) {
                this.footstepTimer -= Time.deltaTime;
            }

            if (this.isGrounded() && this.footstepTimer <= 0 && (forwardSpeed != 0 || sideSpeed != 0)) {
                this.soundSource.PlayOneShot(this.getRndFootstep());
                this.footstepTimer = this.isRunKeyDown() ? 0.3f : 0.5f;
            }
            */
        }

        private float getMoveSpeed() {
            return Input.GetKey(KeyCode.LeftControl) ? 7f : 3f;
        }
    }
}
