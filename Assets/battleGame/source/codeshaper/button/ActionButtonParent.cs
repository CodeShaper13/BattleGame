namespace codeshaper.button {

    public class ActionButtonParent : ActionButton {

        private readonly ActionButton[] childButtons;

        public ActionButtonParent(string actionName, int id, params ActionButton[] buttons) : base(actionName, id) {
            this.childButtons = buttons;
            foreach(ActionButton children in this.childButtons) {
                ((ActionButtonChild)children).parentActionButton = this;
            }
        }

        /// <summary>
        /// Returns an array of all the sub buttons.
        /// </summary>
        public ActionButton[] getChildButtons() {
            return this.childButtons;
        }
    }
}
