using codeshaper.team;
using System;

namespace codeshaper.button {

    public class ActionButtonParent : ActionButton {

        /// <summary>
        /// Called on every child button every Update when they are visable.
        /// Use this to disable buttons if they should not be clicked.
        /// </summary>
        public readonly Func<ActionButton, Team, bool> childUpdateAction;

        private readonly ActionButtonChild[] subButtons;

        public ActionButtonParent(string actionName, int id, params ActionButtonChild[] buttons) : this(actionName, id, null, buttons) { }

        public ActionButtonParent(string actionName, int id, Func<ActionButton, Team, bool> childUpdateAction, params ActionButtonChild[] buttons) : base(actionName, id, null) {
            this.subButtons = buttons;
            foreach(ActionButtonChild children in this.subButtons) {
                children.parentActionButton = this;
            }
            this.childUpdateAction = childUpdateAction;
        }

        /// <summary>
        /// Returns an array of all the sub buttons.
        /// </summary>
        public ActionButton[] getSubButtons() {
            return this.subButtons;
        }
    }
}
