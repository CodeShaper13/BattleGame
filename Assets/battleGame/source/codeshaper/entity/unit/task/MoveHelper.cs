using codeshaper.debug;
using codeshaper.util;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace codeshaper.entity.unit.task {

    public class MoveHelper : IDrawDebug {

        private const float TURN_SPEED = 500f;

        private UnitBase unit;
        private NavMeshAgent agent;

        private Vector3 lastSetCall;

        public MoveHelper(UnitBase unit) {
            this.unit = unit;
            this.agent = this.unit.GetComponent<NavMeshAgent>();
            this.agent.speed = this.unit.unitStats.getSpeed();
            this.agent.angularSpeed = TURN_SPEED;
        }

        /// <summary>
        /// Rotates the Unit's body and head to look at the passed position.
        /// </summary>
        public void lookAt(Vector3 pos) {
            throw new NotImplementedException();
        }

        public void setDestination(LivingObject sidedObjectEntity) {
            this.setDestination(sidedObjectEntity.getPos(), sidedObjectEntity.getSizeRadius());
        }

        public void setDestination(Vector3 pos, float stopingDistance = -1) {
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
        /// Stops the unit where they are.  If stopImmediately is true, the agent's velocity is set to 0, freezing it instantly.
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
