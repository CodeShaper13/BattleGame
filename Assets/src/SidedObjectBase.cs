using src.team;
using UnityEngine;

namespace src {

    public abstract class SidedObjectBase : MonoBehaviour {

        public Team team;
        [SerializeField]
        private EnumTeam objectTeam;

        private void Awake() {

            int i = this.getForcedTeam();
            this.team = Team.teamFromEnum(i == -1 ? this.objectTeam : (EnumTeam)i);

            this.onAwake();
        }

        private void Start() {
            this.onStart();
        }

        private void Update() {
            this.onUpdate();
        }

        protected virtual void onAwake() { }

        protected virtual void onStart() { }

        protected virtual void onUpdate() { }

        protected virtual int getForcedTeam() {
            return -1; // Let the inspector pick the team;
        }
    }
}
