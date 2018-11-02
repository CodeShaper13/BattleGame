using codeshaper.buildings;
using codeshaper.entity;
using codeshaper.team;
using System;
using System.Collections.Generic;

namespace codeshaper.button {

    public class ActionButtonRequireClick : ActionButton {

        private Action<SidedObjectEntity, SidedObjectEntity> mainFunctionDelayed;
        private Func<IEnumerable<SidedObjectEntity>, SidedObjectEntity, SidedObjectEntity> entitySelecterFunctionDelayed;

        /// <summary> A function that checks if the passed entity is a valid target for this action. </summary>
        private Func<Team, SidedObjectEntity, bool> validForActionFunction;

        public ActionButtonRequireClick(string actionName, int id) : base(actionName, id) { }

        public ActionButtonRequireClick setMainActionFunction(Action<SidedObjectEntity, SidedObjectEntity> function) {
            this.mainFunctionDelayed = function;
            return this;
        }

        public ActionButtonRequireClick setValidForActionFunction(Func<Team, SidedObjectEntity, bool> function) {
            this.validForActionFunction = function;
            return this;
        }

        public ActionButtonRequireClick setEntitySelecterFunction(Func<IEnumerable<SidedObjectEntity>, SidedObjectEntity, SidedObjectEntity> function) {
            this.entitySelecterFunctionDelayed = function;
            return this;
        }

        public bool isValidForAction(Team team, SidedObjectEntity entity) {
            return this.validForActionFunction(team, entity);
        }

        // Overloads of parent class functions to provide the clicked argument.

        public void callFunction(BuildingBase building, SidedObjectEntity clicked) {
            this.mainFunctionDelayed.Invoke(building, clicked);
        }

        public void callFunction<T>(List<T> list, SidedObjectEntity clickedObject) where T : SidedObjectEntity {
            List<SidedObjectEntity> candidates = this.getCandidates(list);

            if (this.entitySelecterFunctionDelayed == null) {
                // Call function on all.
                foreach (SidedObjectEntity entity in candidates) {
                    this.mainFunctionDelayed.Invoke(entity, clickedObject);
                }
            }
            else {
                SidedObjectEntity e = this.entitySelecterFunctionDelayed.Invoke(candidates, clickedObject);
                this.mainFunctionDelayed.Invoke(e, clickedObject);
            }
        }
    }
}
