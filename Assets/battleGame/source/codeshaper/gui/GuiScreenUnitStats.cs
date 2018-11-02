using codeshaper.entity.unit;
using codeshaper.entity.unit.stats;
using UnityEngine;
using UnityEngine.UI;

namespace codeshaper.gui {

    public class GuiScreenUnitStats : GuiBase {

        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text statText;

        public override void onGuiInit() {
            
        }

        public void set(UnitBase unit) {
            UnitStats stats = unit.unitStats;
            this.nameText.text = stats.getName() + "\n" + unit.getData().getName();
            this.statText.text = stats.getFormattedStatString(unit is UnitBuilder);
            stats.dirty = false;
        }
    }
}
