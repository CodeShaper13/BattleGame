using src.team;
using System;
using UnityEngine;

namespace src.entity {

    /// <summary>
    /// Represents and object that belongs on a certain side.
    /// </summary>
    public abstract class SidedObjectBase : MapObject {

        private Team team;
        [SerializeField] // Show in Inspector, but private so script won't change it.
        private EnumTeam objectTeam;

        protected override void onStart() {
            base.onStart();

            int i = this.getForcedTeam();
            this.team = Team.teamFromEnum(i == -1 ? this.objectTeam : (EnumTeam)i);

            if (!(this is CameraMover)) {
                this.team.join(this);
            }
        }

        /// <summary>
        /// Returns the team that this object is on.
        /// </summary>
        public Team getTeam() {
            return this.team;
        }

        public void setTeam(Team newTeam) {
            print("setting team to " + newTeam.getName());
            if(!(this.team == null || this.team == Team.NONE)) {
                throw new Exception("Can not change objects team!");
            }

            if(this.team != null) {
                this.team.leave(this);
            }

            newTeam.join(this);
            this.team = newTeam;
            // Used to help debug int he inspector
            this.objectTeam = this.team.getEnum();
        }

        /// <summary>
        /// Override this method to force the object to belong on a certina team and to ignore the inspector.
        /// </summary>
        protected virtual int getForcedTeam() {
            return -1; // Let the inspector pick the team;
        }
    }
}
