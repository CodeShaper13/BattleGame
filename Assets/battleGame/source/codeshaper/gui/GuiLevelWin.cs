namespace codeshaper.gui {

    public class GuiLevelWin : GuiBase {

        public void callback_exit() {
            GuiManager.closeCurrentGui();

            Main.instance().loadTownScene();
        }
    }
}
