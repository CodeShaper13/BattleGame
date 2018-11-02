using codeshaper.debug;
using codeshaper.util;
using UnityEngine;
using UnityEngine.AI;

namespace codeshaper.entity.unit.task {

    public class MoveHelper : IDrawDebug {

        private UnitBase unit;
        private NavMeshAgent agent;

        private Vector3 lastSetCall;

        public MoveHelper(UnitBase unit) {
            this.unit = unit;
            this.agent = this.unit.GetComponent<NavMeshAgent>();

        }

        public void lookAt(Vector3 pos) {

        }

        public void setDestination(SidedObjectEntity sidedObjectEntity) {
            this.setDestination(sidedObjectEntity.getPos(), sidedObjectEntity.getSizeRadius() + this.unit.getSizeRadius() + 0.5f);
        }

        public void setDestination(Vector3 pos, float stopingDistance) {
            if(pos != this.lastSetCall && Vector3.Distance(pos, this.lastSetCall) >= 0.1f) {
                this.agent.isStopped = false;
                this.agent.SetDestination(pos);
                
                if (stopingDistance != -1) {
                    this.agent.stoppingDistance = stopingDistance;
                }
                else {
                    this.agent.stoppingDistance = 0.5f; //TODO should this be a setting?
                }
            }

            this.lastSetCall = pos;
        }

        /// <summary>
        /// Stops the unit where they are.
        /// </summary>
        public void stop(bool stopImmediately = false) {
            if(!this.agent.isStopped) {
                this.agent.isStopped = true;
                if (stopImmediately) {
                    this.agent.velocity = Vector3.zero;
                }
            }
        }

        public void drawDebug() {
            GLDebug.DrawLine(this.agent.transform.position.setY(0.5f), this.agent.destination.setY(0.5f), Color.cyan);
        }
    }
}
