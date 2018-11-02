using cakeslice;
using UnityEngine;

namespace codeshaper.util.outline {

    public class OutlineHelper {

        private bool outline1;
        private bool outline2;
        private bool outline3;

        private Outline outline;

        public OutlineHelper(GameObject obj) {
            this.outline = obj.GetComponent<Outline>();
        }

        public bool isVisible(EnumOutlineType type) {
            switch (type) {
                case EnumOutlineType.SELECTED:
                    return this.outline1;
                case EnumOutlineType.ACTION_OPTION:
                    return this.outline2;
                case EnumOutlineType.RED:
                    return this.outline3;
            }
            return false;
        }

        public void updateOutline(bool visible, EnumOutlineType type) {
            switch(type) {
                case EnumOutlineType.ALL:
                    this.outline1 = visible;
                    this.outline2 = visible;
                    this.outline3 = visible;
                    break;
                case EnumOutlineType.SELECTED:
                    this.outline1 = visible;
                    break;
                case EnumOutlineType.ACTION_OPTION:
                    this.outline2 = visible;
                    break;
                case EnumOutlineType.RED:
                    this.outline3 = visible;
                    break;
            }

            // Update the one you can see.
            if(this.outline1) {
                this.outline.color = 0;
                this.outline.enabled = true;
            } else if(this.outline2) {
                this.outline.color = 1;
                this.outline.enabled = true;
            }
            else if(this.outline3) {
                this.outline.color = 2;
                this.outline.enabled = true;
            }
            else {
                this.outline.enabled = false;
            }
        }
    }
}
