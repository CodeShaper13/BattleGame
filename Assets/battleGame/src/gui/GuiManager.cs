using UnityEngine;

namespace src.gui {

    public class GuiManager {

        public static GuiScreen paused;
        public static GuiScreen unitStats;

        /// <summary>
        /// The gui that is currently open.
        /// </summary>
        [HideInInspector]
        public static GuiScreen currentGui;

        /// <summary>
        /// Opens the passed gui, closing any previously open ones.
        /// </summary>
        public static GuiScreen openGui(GuiScreen newGui) {
            if (GuiManager.currentGui != null) {
                // Hide the current gui screen, only if there is one.
                // In the event of pressing pause while playing or when opening the
                // title screen there is no current gui.
                GuiManager.currentGui.gameObject.SetActive(false);
            }
            GuiManager.currentGui = newGui;
            GuiManager.currentGui.gameObject.SetActive(true);

            return newGui;
        }

        public static void closeCurrentGui() {
            GuiManager.currentGui.gameObject.SetActive(false);
            GuiManager.currentGui = null;
        }
 
        public static void guiBootstrap() {
            GuiManager.paused = References.list.guiPausedObject.GetComponent<GuiScreen>();
            GuiManager.unitStats = References.list.guiUnitStatsObject.GetComponent<GuiScreen>();
        }
    }
}
