using UnityEngine;

namespace codeshaper.gui {

    public class GuiManager {

        public static GuiBase credits;
        public static GuiBase paused;
        public static GuiBase unitStats;
        public static GuiBase levelFail;
        public static GuiBase levelWin;
        public static GuiBase findUnlockable;
        public static GuiBase titleScreen;

        /// <summary>
        /// The gui that is currently open.
        /// </summary>
        [HideInInspector]
        public static GuiBase currentGui;

        /// <summary>
        /// Opens the passed gui, closing any previously open ones.
        /// </summary>
        public static GuiBase openGui(GuiBase newGui) {
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

        /// <summary>
        /// Closes the current gui, hiding it.
        /// </summary>
        public static void closeCurrentGui() {
            GuiManager.currentGui.gameObject.SetActive(false);
            GuiManager.currentGui = null;
        }
 
        public static void guiBootstrap() {
            GuiManager.credits = References.list.guiCreditsObject;
            GuiManager.paused = References.list.guiPausedObject;
            GuiManager.unitStats = References.list.guiUnitStatsObject;
            GuiManager.levelFail = References.list.guiLevelFailObject;
            GuiManager.levelWin = References.list.guiLevelWinObject;
            GuiManager.findUnlockable = References.list.guiFindUnlockableObject;
            GuiManager.titleScreen = References.list.guiTitleScreenObject;
        }
    }
}
