using cakeslice;
using fNbt;
using codeshaper.button;
using codeshaper.registry;
using codeshaper.team;
using codeshaper.util;
using UnityEngine;

namespace codeshaper.entity {

    /// <summary>
    /// Represents an object that belongs to a certain side.
    /// </summary>
    public abstract class SidedObjectEntity : LivingObject, IHasOutline {

        [SerializeField] // Show in Inspector, but private so script won't change it.
        private EnumTeam objectTeam;
        private Team team;
        private Outline outline;

        protected override void onAwake() {
            base.onAwake();

            this.outline = this.GetComponent<Outline>();
        }

        protected override void onStart() {
            base.onStart();

            this.setOutlineVisibility(false);

            if(this.objectTeam != EnumTeam.NONE) {
                this.setTeam(Team.teamFromEnum(this.objectTeam));
            } else {
                this.setTeam(Team.NONE);
            }
        }

        public override void onDeathCallback() {
            base.onDeathCallback();

            this.getTeam().leave(this);
        }

        /// <summary>
        /// Returns the bitmask of what buttons to display.
        /// </summary>
        public virtual int getButtonMask() {
            return ActionButton.destroy.mask;
        }

        /// <summary>
        /// Overried to stop the unit from allowing the action buttons to be pressed
        /// depending on the units state.
        /// 
        /// Is this implemented?
        /// </summary>
        public virtual bool enableActionButton() {
            return true;
        }

        public abstract float getSizeRadius();

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            // Do not read id tag.
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("id", Registry.getIdFromObject(this));
        }

        /// <summary>
        /// Returns the team that this object is on.
        /// </summary>
        public Team getTeam() {
            return this.team;
        }

        /// <summary>
        /// Sets the team that this Object is on, preforming required departing and joining actions.
        /// </summary>
        public void setTeam(Team newTeam) {
            // Leave the old team if there was one.
            if (this.team != null) {
                this.team.leave(this);
            }

            this.team = newTeam;
            this.team.join(this);

            // Used to help debug in the inspector, so we can see the objects team.
            this.objectTeam = this.team.getEnum();
        }

        public virtual void setOutlineVisibility(bool visible) {
            this.outline.enabled = visible;
        }
    }
}
