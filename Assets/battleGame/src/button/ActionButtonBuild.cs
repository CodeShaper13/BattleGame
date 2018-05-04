using UnityEngine;

namespace src.button {

    public class ActionButtonBuild : ActionButton {

        public ActionButtonBuild(string actionName, GameObject prefab) : base(actionName, -1) {
            this.function = (unit) => {
                BuildOutline.reference.enableOutline(prefab);
            };
        }
    }
}
