using codeshaper.item;
using UnityEngine;
using UnityEngine.UI;

namespace codeshaper.gui {

    public class GuiUnlockable : GuiBase {

        public Transform itemPos;

        public Text itemName;
        public Text itemDescription;

        public void setItem(Item item) {
            this.itemName.text = item.getName();
            this.itemDescription.text = item.getDescription();

            // TODO show item.
        }

        public void callback_ok() {
            GuiManager.closeCurrentGui();
        }
    }
}
