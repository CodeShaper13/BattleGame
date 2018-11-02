namespace codeshaper.gui {

    public class GuiLevelFail : GuiBase {

        public void callback_exit() {
            GuiManager.closeCurrentGui();

            Main.instance().loadTownScene();
        }

        public void callback_retray() {
            GuiManager.closeCurrentGui();

            Main.instance().gameState.getCurrentLevel().loadLevelScene();
        }
    }
}
