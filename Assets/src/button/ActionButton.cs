using System;
using UnityEngine;

namespace src.button {

    // Note, sub buttons do not need an id nor do they need to be in the list.

    public class ActionButton {

        public static readonly ActionButton[] buttonList = new ActionButton[64];

        // General (0-7).
        public static readonly ActionButton destroy = new ActionButton("Destroy", 0, (unit) => {
            Debug.Log("!!!");
            unit.damage(int.MaxValue);
        });

        // Units (8-15);
        public static readonly ActionButton builderBuild = new ActionButtonParent("Build", 1,
            new ActionButtonBuild("Farm", References.list.buildingFarm),
            new ActionButtonBuild("Workshop", References.list.buildingWorkshop),
            new ActionButtonBuild("Training House", References.list.buildingTrainingHouse),
            new ActionButtonBuild("Tower", References.list.buildingTower),
            new ActionButtonBuild("Wall", References.list.buldingWall));

        public static readonly ActionButton harvest = new ActionButton("Harvest", 2);

        public static readonly ActionButton holdGround = new ActionButton("Build", 3);

        // Buildings (16-23)
        public static readonly ActionButton upgrade = new ActionButton("Upgrade", 16);

        public static readonly ActionButton train = new ActionButtonParent("Train", 17,
            new ActionButton("Train Soldier", 24),
            new ActionButton("Train Archer", 25),
            new ActionButton("Train Heavy", 26),
            new ActionButton("Train Builder", 27));


        public readonly string name;
        public Action<SidedObjectEntity> function;

        private readonly int id;
        private readonly int mask;

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

        public int getMask() {
            return mask;
        }
    }
}
