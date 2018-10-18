using codeshaper.entity;
using System;

namespace codeshaper.button {

    public class ActionButtonChild : ActionButton {

        private const int CHILD_ID = -1;

        public ActionButtonParent parentActionButton;

        public ActionButtonChild(string actionName) : base(actionName, CHILD_ID) {
        }

        public ActionButtonChild(string actionName, Action<SidedObjectEntity> action) : base(actionName, CHILD_ID, action) {
        }
    }
}
