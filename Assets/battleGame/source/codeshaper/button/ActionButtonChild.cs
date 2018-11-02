namespace codeshaper.button {

    public class ActionButtonChild : ActionButton {

        public const int CHILD_ID = -1;

        public ActionButtonParent parentActionButton;

        public ActionButtonChild(string actionName) : base(actionName, CHILD_ID) { // There is no action, it is set in the constructor.
        }

        public override int getMask() {
            return this.parentActionButton.getMask();
        }
    }
}
