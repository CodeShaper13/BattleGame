using fNbt;
using codeshaper.button;
using codeshaper.team;
using UnityEngine;
using codeshaper.util.outline;
using codeshaper.nbt;
using System;

namespace codeshaper.entity {

    /// <summary>
    /// Represents an object that belongs to a certain side.
    /// </summary>
    public abstract class SidedObjectEntity : LivingObject, IHasOutline {

        [SerializeField] // Show in Inspector, but private so script won't change it.
        private EnumTeam objectTeam;
        private Team team;
        private OutlineHelper outlineHelper;

        protected override void onAwake() {
            base.onAwake();

            this.outlineHelper = new OutlineHelper(this.gameObject);

            //TODO do we need this?
            this.setTeam(Team.getTeamFromEnum(this.objectTeam));
        }

        protected override void onStart() {
            base.onStart();

            this.gameObject.name = "[" + this.team.getName() + "] " + this.gameObject.name;
            this.colorUnit();

            //TODO move on onAwake()?
            this.setOutlineVisibility(false, EnumOutlineParam.ALL);
        }

        public override void onDeathCallback() {
            base.onDeathCallback();
        }

        /// <summary>
        /// Returns the bitmask of what buttons to display.
        /// </summary>
        public virtual int getButtonMask() {
            return ActionButton.destroy.getMask();
        }

        /// <summary>
        /// Colors the unit based on it's team.
        /// </summary>
        protected virtual void colorUnit() { }

        /// <summary>
        /// Overried to stop the unit from allowing the action buttons to be pressed
        /// depending on the units state.
        /// </summary>
        //TODO not implemented.
        public virtual bool enableActionButton() {
            throw new NotImplementedException();
        }

        public override bool shouldHealthbarBeShown() {
            return base.shouldHealthbarBeShown() || this.outlineHelper.isOutlineVisible(EnumOutlineParam.SELECTED);
        }

        public override void readFromNbt(NbtCompound tag) {
            base.readFromNbt(tag);

            this.team = Team.getTeamFromId(tag.getInt("teamId"));
        }

        public override void writeToNbt(NbtCompound tag) {
            base.writeToNbt(tag);

            tag.setTag("teamId", this.team.getTeamId());
        }

        /// <summary>
        /// Returns the team that this object is on.
        /// </summary>
        public Team getTeam() {
            return this.team;
        }

        /// <summary>
        /// Sets the team that this Entity is on.
        /// </summary>
        public void setTeam(Team newTeam) {
            this.team = newTeam;
            // Used to help debug in the inspector, so we can see the objects team.
            this.objectTeam = this.team.getEnum();
        }

        /// <summary>
        /// Sets if the outline is visible.  An optional number can be passed to set the outline color.
        /// </summary>
        public virtual void setOutlineVisibility(bool visible, EnumOutlineParam type) {
            this.outlineHelper.updateOutline(visible, type);
        }
    }
}
