using src.troop;
using src.troop.task;
using src.troop.task.builder;
using System;

namespace src.button {

    // Note, sub buttons do not need an id nor do they need to be in the list.

    public class ActionButton {

        public static readonly ActionButton[] buttonList = new ActionButton[64];

        /// <summary>
        /// Destroys the Unit, removing it from the game.
        /// </summary>
        public static readonly ActionButton destroy = new ActionButton("Destroy Unit", 0, (unit) => {
            unit.damage(int.MaxValue);
        });

        public static readonly ActionButton builderBuild = new ActionButtonParent("Build", 1,
            new ActionButtonBuild("Camp", References.list.buildingCamp),
            new ActionButtonBuild("Workshop", References.list.buildingWorkshop),
            new ActionButtonBuild("Training House", References.list.buildingTrainingHouse),
            new ActionButtonBuild("Storeroom", References.list.buildingStoreroom),
            new ActionButtonBuild("Tower", References.list.buildingTower),
            new ActionButtonBuild("Wall", References.list.buldingWall));


        public static readonly ActionButton harvestResources = new ActionButton("Harvest", 2, (unit) => {
            UnitBuilder ub = ((UnitBuilder)unit);
            ub.setTask(new TaskHarvestNearby(ub));
        });

        // Fighting troop attacks.
        public static readonly ActionButton idle = new ActionButton("Idle", 7, (unit) => {
            UnitFighting uf = ((UnitFighting)unit);
            uf.setDestination(null);
            uf.setTask(null);
        });
        public static readonly ActionButton attackNearby = new ActionButton("Attack", 8, (unit) => {
            UnitFighting uf = ((UnitFighting)unit);
            uf.setTask(new TaskAttackNearby(uf));
        });
        public static readonly ActionButton defend = new ActionButton("Defend Unit", 9, (unit) => {
            UnitFighting uf = ((UnitFighting)unit);
            uf.setTask(new TaskDefendPoint(uf));
        });

        // Buildings (16-23)
        public static readonly ActionButton upgrade = new ActionButton("Upgrade", 16);

        public static readonly ActionButton train = new ActionButtonParent("Train", 17,
            new ActionButton("Train Soldier", 24),
            new ActionButton("Train Archer", 25),
            new ActionButton("Train Heavy", 26),
            new ActionButton("Train Builder", 27));


        public Action<SidedObjectEntity> function;
        public readonly int mask;

        private readonly int id;
        private readonly string name;

        public ActionButton(string actionName, int id) {
            this.name = actionName;
            this.id = id;
            this.mask = (1 << this.id);

            if (id >= 0) {
                ActionButton.buttonList[this.id] = this;
            }
        }

        public ActionButton(string actionName, Action<SidedObjectEntity> action) : this(actionName, -1, action) { }

        public ActionButton(string actionName, int id, Action<SidedObjectEntity> action) : this(actionName, id) {
            this.function = action;
        }

        /// <summary>
        /// Returns the text to display on the button.
        /// </summary>
        /// <returns></returns>
        public string getText() {
            return this.name;
        }
    }
}
