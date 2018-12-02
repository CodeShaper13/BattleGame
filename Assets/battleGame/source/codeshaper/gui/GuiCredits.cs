using codeshaper.util;
using System.Collections.Generic;
using UnityEngine.UI;

namespace codeshaper.gui {

    public class GuiCredits : GuiBase {

        public Text creditText;

        public override void onGuiInit() {
            List<string> lines = FileUtils.readTextAsset(References.list.textCredits, true);
            string s = string.Empty;
            foreach(string s1 in lines) {
                s += s1;
            }
            this.creditText.text = s;
        }
    }
}
