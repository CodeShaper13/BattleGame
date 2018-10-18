using codeshaper.registry;
using codeshaper.entity.unit;
using codeshaper.entity.unit.task;
using codeshaper.entity.unit.task.builder;
using System;
using codeshaper.entity;
using UnityEngine;
using codeshaper.buildings;

namespace codeshaper.button {

    // Note, sub buttons do not need an id nor do they need to be in the list.

    public class ActionButton {

        public static readonly ActionButton[] buttonList = new ActionButton[64];

        /// <summary> Destroys the Unit, removing it from the game. </summary>
        public static readonly ActionButton destroy = new ActionButtonParent("Destroy", 0,
            new ActionButtonChild("Confirm", (unit) => { unit.damage(int.MaxValue); }));

        // Builder specific.
        public static readonly ActionButton builderBuild = new ActionButtonParent("Build", 1, (childButton, team) => {
            ActionButtonBuild abb = (ActionButtonBuild)childButton;
            int cost = abb.getBuildingData().getCost();
            return cost <= team.getResources();
        },
            new ActionButtonBuild("Camp", Registry.buildingCamp),
            new ActionButtonBuild("Producer", Registry.buildingProducer),
            new ActionButtonBuild("Workshop", Registry.buildingWorkshop),
            new ActionButtonBuild("Training House", Registry.buildingTrainingHouse),
            new ActionButtonBuild("Storeroom", Registry.buildingStoreroom),
            new ActionButtonBuild("Tower", Registry.buildingTower),
            new ActionButtonBuild("Wall", Registry.buldingWall)
        );

        public static readonly ActionButton harvestResources = new ActionButton("Harvest", 2, (unit) => {
            UnitBuilder ub = ((UnitBuilder)unit);
            ub.setTask(new TaskHarvestNearby(ub));
        });
        public static readonly ActionButton repairBuilder = new ActionButton("Repair Building", 3, (unit) => {
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

        public static readonly ActionButton train = new ActionButtonParent("Train", 17, (childButton, team) => {
            ActionButtonTrain abb = (ActionButtonTrain)childButton;
                        
            return abb.getEntityData().getCost() <= team.getResources();
        },
        new ActionButtonTrain(Registry.unitSoldier),
            new ActionButtonTrain(Registry.unitArcher),
            new ActionButtonTrain(Registry.unitHeavy),
            new ActionButtonTrain(Registry.unitBuilder));

        public static readonly ActionButton buildingRotate = new ActionButton("Rotate", 20, (building) => {
            BuildingBase b = (BuildingBase)building;
            b.rotateBuilding();

        });
        public static readonly ActionButton buildSpecial = new ActionButtonParent("Construct", 21, (childButton, team) => {
            ActionButtonTrain abb = (ActionButtonTrain)childButton;
            int cost = abb.getEntityData().getCost();
            return cost <= team.getResources();
        },
            new ActionButtonTrain(Registry.specialWarWagon),
            new ActionButtonTrain(Registry.specialCannon)
        );


        /// <summary> The function that is run when the button is clicked. </summary>
        public Action<SidedObjectEntity> function;
        /// <summary> The mask of the action button, this is -1 on child buttons. </summary>
        public readonly int mask;

        private readonly int id;
        private readonly string displayName;

        public ActionButton(string actionName, int id) :  this(actionName, id, (unit) => { Debug.Log("Default Callback.  It is likely there is an error somewhere."); }) { }

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
        /// Returns the text to display on the button.  Override to provide fancier text.
        /// </summary>
        /// <returns></returns>
        public virtual string getText() {
            return this.displayName;
        }
    }
}
