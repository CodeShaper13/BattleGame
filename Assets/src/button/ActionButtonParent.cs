using System;

namespace src.button {

    public class ActionButtonParent : ActionButton {

        private ActionButton[] subButtons;

        public ActionButtonParent(string actionName, int id, params ActionButton[] buttons) : base(actionName, id, null) {
            this.subButtons = buttons;
        }

        public ActionButton[] getSubButtons() {
            return this.subButtons;
        }
    }
}
