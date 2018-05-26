using src.entity.unit;
using src.entity.unit.stats;
using UnityEngine;
using UnityEngine.UI;

namespace src.gui {

    public class GuiScreenUnitStats : GuiScreen {

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
