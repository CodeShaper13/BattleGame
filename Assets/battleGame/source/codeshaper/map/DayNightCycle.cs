using codeshaper.nbt;
using fNbt;
using UnityEngine;
using codeshaper.util;
using UnityEngine.Experimental.GlobalIllumination;

namespace codeshaper.map {

    public class DayNightCycle : MonoBehaviour, INbtSerializable {

        private const int SECONDS_IN_DAY = 1200;

        [SerializeField]
        private int daysElapsed;
        [SerializeField]
        [Range(0, SECONDS_IN_DAY - 1)]
        private float currentTime;
        [SerializeField]
        private bool stopTime;
        public DirectionalLight sun;

        private float startingYRot;

        private void Awake() {
            this.startingYRot = this.transform.eulerAngles.y;
            this.transform.position.setY(0);
        }

        private void Update() {
            if(!this.stopTime) {
                this.currentTime += Time.deltaTime;
            }

            if(this.currentTime >= SECONDS_IN_DAY) {
                this.currentTime = 0;
                this.daysElapsed += 1;
            }

            this.transform.eulerAngles = new Vector3((360f / SECONDS_IN_DAY) * (currentTime + 60), 0, 0);
        }

        private void OnGUI() {
            if(Main.DEBUG) {
                GUI.Label(new Rect(10, 10, 100, 20), "TIME: " + this.currentTime + " DAY: " + this.daysElapsed);
            }
        }

        public void startCycle() {
            this.stopTime = false;
        }

        public void stopCycle() {
            this.stopTime = true;
        }

        /// <summary>
        /// Returns true if it is night time.
        /// </summary>
        /// <returns></returns>
        public bool isNight() {
            return this.currentTime > (SECONDS_IN_DAY / 2);
        }

        public int getDay() {
            return this.daysElapsed;
        }

        public float getTime() {
            return this.currentTime;
        }

        public void readFromNbt(NbtCompound tag) {
            NbtCompound tag1 = tag.getCompound("time");

            this.daysElapsed = tag.getInt("daysElapsed");
            this.currentTime = tag.getInt("currentTime");
        }

        public void writeToNbt(NbtCompound tag) {
            NbtCompound tag1 = new NbtCompound("time");

            tag1.setTag("daysElapsed", this.daysElapsed);
            tag1.setTag("currentTime", this.currentTime);

            tag.Add(tag1);
        }
    }
}
