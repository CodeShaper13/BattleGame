using UnityEngine;
using UnityEngine.AI;

namespace src.troop {

    public abstract class UnitBase : SidedObjectEntity {

        private NavMeshAgent agent;

        protected override void onAwake() {
            base.onAwake();
            this.agent = this.GetComponent<NavMeshAgent>();
        }

        public override void onDeathCallback() {
            base.onDeathCallback();
            CameraMover.singleton.party.remove(this);
            GameObject.Destroy(this.gameObject);
        }

        public void setDestination(Vector3 point) {
            this.agent.SetDestination(point);
        }

        public abstract string getUnitName();
    }
}
