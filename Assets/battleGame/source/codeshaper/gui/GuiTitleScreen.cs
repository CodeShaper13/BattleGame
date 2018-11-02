using codeshaper.save;
using codeshaper.util;
using UnityEngine.UI;

namespace codeshaper.gui {

    public class GuiTitleScreen : GuiBase {

        public Text firstBtnText;

        private bool saveFileExists;

        public override void onGuiInit() {
            this.saveFileExists = Util.doesSaveExists();

            this.firstBtnText.text = this.saveFileExists ? "Continue" : "Start New Game";
        }

        public override void onGuiOpen() {
            base.onGuiOpen();
        }

        public void callback_firstBtnClick() {
            GuiManager.closeCurrentGui();

            if (this.saveFileExists) {
                Main.instance().gameState = new GameState(Main.SAVE_PATH);
            } else {
                Main.instance().startNewGame();
            }
        }

        public void callback_deleteSave() {
            GuiManager.closeCurrentGui();

            // TODO delete files

            Main.instance().startNewGame();
        }
    }
}
