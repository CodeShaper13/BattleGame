namespace src.button {

    public class ActionButtonParent : ActionButton {

        private readonly ActionButton[] subButtons;

        public ActionButtonParent(string actionName, int id, params ActionButton[] buttons) : base(actionName, id, null) {
            this.subButtons = buttons;
        }

        /// <summary>
        /// Returns an array of all the sub buttons.
        /// </summary>
        public ActionButton[] getSubButtons() {
            return this.subButtons;
        }
    }
}
