using codeshaper.buildings;
using codeshaper.util;

namespace codeshaper.map.gamemode {

    public class GameModeDTM : GameModeBase {

        public BuildingBase monument;

        protected override string getGameModeName() {
            return "Destroy the Monument";
        }

        protected override string getDescription() {
            return "TODO";
        }

        protected override void updateGameMode() {
            if(Util.isAlive(this.monument)) {
                this.triggerWin();
            }
        }
    }
}
