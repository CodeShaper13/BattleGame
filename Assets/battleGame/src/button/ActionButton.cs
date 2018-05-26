using src.registry;
using src.entity.unit;
using src.entity.unit.task;
using src.entity.unit.task.builder;
using System;
using src.entity;
using UnityEngine;

namespace src.button {

    // Note, sub buttons do not need an id nor do they need to be in the list.

    public class ActionButton {

        public static readonly ActionButton[] buttonList = new ActionButton[64];

        /// <summary> Destroys the Unit, removing it from the game. </summary>
        public static readonly ActionButton destroy = new ActionButtonParent("Destroy Unit", 0,
            new ActionButton("Confirm", 0, (unit) => { unit.damage(int.MaxValue); }));

        public static readonly ActionButton builderBuild = new ActionButtonParent("Build", 1,
            new ActionButtonBuild("Camp", Registry.buildingCamp),
            new ActionButtonBuild("Workshop", Registry.buildingWorkshop),
            new ActionButtonBuild("Training House", Registry.buildingTrainingHouse),
            new ActionButtonBuild("Storeroom", Registry.buildingStoreroom),
            new ActionButtonBuild("Tower", Registry.buildingTower),
            new ActionButtonBuild("Wall", Registry.buldingWall));

        // Builder specific.
        public static readonly ActionButton harvestResources = new ActionButton("Harvest", 2, (unit) => {
            UnitBuilder ub = ((UnitBuilder)unit);
            ub.setTask(new TaskHarvestNearby(ub));
        });
        public static readonly ActionButton repairBuilder = new ActionButton("Repair Buildin", 3, (unit) => {
            UnitBuilder ub = ((UnitBuilder)unit);
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
            new ActionButtonTrain(Registry.unitSoldier),
            new ActionButtonTrain(Registry.unitArcher),
            new ActionButtonTrain(Registry.unitHeavy),
            new ActionButtonTrain(Registry.unitBuilder));


        /// <summary> The function that is run when the button is clicked. </summary>
        public Action<SidedObjectEntity> function;
        public readonly int mask;

        private readonly int id;
        private readonly string displayName;

        public ActionButton(string actionName, int id) :  this(actionName, id, (unit) => { Debug.Log("Default Callback"); }) { }

        public ActionButton(string actionName, Action<SidedObjectEntity> action) : this(actionName, -1, action) { }

        public ActionButton(string actionName, int id, Action<SidedObjectEntity> action) {
            this.displayName = actionName;
            this.id = id;
            this.function = action;
            this.mask = (1 << this.id);

            if (id >= 0) {
                ActionButton.buttonList[this.id] = this;
            }
        }

        /// <summary>
        /// Returns the text to display on the button.
        /// </summary>
        /// <returns></returns>
        public virtual string getText() {
            return this.displayName;
        }
    }
}
