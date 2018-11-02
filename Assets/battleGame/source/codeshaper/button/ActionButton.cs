using codeshaper.registry;
using codeshaper.entity.unit;
using codeshaper.entity.unit.task;
using codeshaper.entity.unit.task.builder;
using System;
using codeshaper.entity;
using codeshaper.buildings;
using System.Collections.Generic;
using codeshaper.util;

namespace codeshaper.button {

    // Note, sub buttons do not need an ID nor do they need to be in the list.

    public class ActionButton {

        public static readonly ActionButton[] buttonList = new ActionButton[64]; // There doesn't seem to be any limit to the size.

        // Functions to use for this.setShouldDisableFunction()
        private static bool functionIsImmutable(SidedObjectEntity entity) { return entity.isImmutable(); }


        public static readonly ActionButton destroy = new ActionButtonParent("Destroy", 0,
            new ActionButtonChild("Confirm")
            .setMainActionFunction((unit) => { unit.damage(null, int.MaxValue); }))
            .setShouldDisableFunction(functionIsImmutable);

        // Builder specific.
        public static readonly ActionButton builderBuild = new ActionButtonParent("Build", 1,
            new ActionButtonBuild("Camp", Registry.buildingCamp),
            new ActionButtonBuild("Producer", Registry.buildingProducer),
            new ActionButtonBuild("Workshop", Registry.buildingWorkshop),
            new ActionButtonBuild("Training House", Registry.buildingTrainingHouse),
            new ActionButtonBuild("Storeroom", Registry.buildingStoreroom),
            new ActionButtonBuild("Tower", Registry.buildingTower),
            new ActionButtonBuild("Wall", Registry.buldingWall)
        );

        public static readonly ActionButton harvestResources = new ActionButton("Harvest", 2)
            .setMainActionFunction((unit) => {
                UnitBuilder ub = ((UnitBuilder)unit);
                ub.setTask(new TaskHarvestNearby(ub));
            });

        public static readonly ActionButton repair = new ActionButtonRequireClick("Repair", 3)
            .setMainActionFunction((unit, target) => {
                UnitBuilder ub = ((UnitBuilder)unit);
                ub.setTask(new TaskRepair(ub, (BuildingBase)target)); //TODO
            })
            .setValidForActionFunction((team, entity) => {
                if (entity.getTeam() == team && entity is BuildingBase) {
                    BuildingBase b = (BuildingBase)entity;
                    return true;
                    if (!b.isConstructing() && b.getHealth() < b.getMaxHealth()) {
                        return true;
                    }
                }
                return false;
            })
            .setEntitySelecterFunction((list, clickedEntity) => {
                return Util.closestToPoint(clickedEntity.getPos(), list, (entity) => { return ((UnitBase)entity).getTask().cancelable(); });
            });

        // Fighting troop attacks.
        public static readonly ActionButton idle = new ActionButton("Idle", 7)
            .setMainActionFunction((unit) => {
                ((UnitBase)unit).setTask(null);
            });

        public static readonly ActionButton attackNearby = new ActionButton("Attack", 8)
            .setMainActionFunction((unit) => {
                UnitFighting uf = ((UnitFighting)unit);
                uf.setTask(new TaskAttackNearby(uf));
            });

        public static readonly ActionButton defend = new ActionButton("Defend Point", 9)
            .setMainActionFunction((unit) => {
                UnitFighting uf = ((UnitFighting)unit);
                uf.setTask(new TaskDefendPoint(uf));
            });

        // Buildings (16-23)
        public static readonly ActionButton upgrade = new ActionButton("Upgrade", 16); //TODO implement.

        public static readonly ActionButton train = new ActionButtonParent("Train", 17,
            new ActionButtonTrain(Registry.unitSoldier),
            new ActionButtonTrain(Registry.unitArcher),
            new ActionButtonTrain(Registry.unitHeavy),
            new ActionButtonTrain(Registry.unitBuilder));

        public static readonly ActionButton buildingRotate = new ActionButton("Rotate", 20)
            .setMainActionFunction((building) => {
                ((BuildingBase)building).rotateBuilding();
            }).setShouldDisableFunction(functionIsImmutable);

        public static readonly ActionButton buildSpecial = new ActionButtonParent("Construct", 21,
            new ActionButtonTrain(Registry.specialWarWagon),
            new ActionButtonTrain(Registry.specialCannon));



        /// <summary> The mask of the action button, this is -1 on child buttons. </summary>
        private readonly int mask;
        private readonly int id;
        private readonly string displayName;

        /// <summary>
        /// The function that is run when the button is clicked.
        /// </summary>
        private Action<SidedObjectEntity> mainFunction;
        /// <summary>
        /// A function used to pick what single entity to call the function on.  This can be
        /// null and if it is, the main function is called on all.
        /// </summary>
        private Func<IEnumerable<SidedObjectEntity>, SidedObjectEntity> entitySelecterFunction;
        /// <summary>
        /// Called on every button every Update when they are visable.
        /// Use this to disable buttons if they should not be clicked.
        /// </summary>
        private Func<SidedObjectEntity, bool> shouldDisableFunction;

        public ActionButton(string actionName) : this(actionName, ActionButtonChild.CHILD_ID) { }

        public ActionButton(string actionName, int id) {
            this.displayName = actionName;
            this.id = id;
            this.mask = (1 << this.id);

            // Only add parent ActionButtons to this list.
            if (id >= 0) {
                ActionButton.buttonList[this.id] = this;
            }
        }

        #region Constructor methods:

        public ActionButton setMainActionFunction(Action<SidedObjectEntity> function) {
            this.mainFunction = function;
            return this;
        }

        public ActionButton setEntitySelecterFunction(Func<IEnumerable<SidedObjectEntity>, SidedObjectEntity> function) {
            this.entitySelecterFunction = function;
            return this;
        }

        public ActionButton setShouldDisableFunction(Func<SidedObjectEntity, bool> function) {
            this.shouldDisableFunction = function;
            return this;
        }

        #endregion

        /// <summary>
        /// Returns true if the action buttons should be disabled.
        /// </summary>
        public bool shouldDisable(SidedObjectEntity thisEntity) {
            if(this.shouldDisableFunction == null) {
                return false;
            } else {
                return this.shouldDisableFunction.Invoke(thisEntity);
            }
        }

        /// <summary>
        /// Calls this button's function on the passed SidedObjectEntity.
        /// </summary>
        public void callFunction(BuildingBase entity) {
            // No need to check masks, as this is a building and there is only ever one selected at a time.
            this.mainFunction.Invoke(entity);
        }

        /// <summary>
        /// Calls the passed function on all of the passed SidedObjectEntities, or a specific
        /// one if this.entitySelecter is not null.
        /// </summary>
        public virtual void callFunction<T>(List<T> list) where T : SidedObjectEntity {
            List<SidedObjectEntity> candidates = this.getCandidates(list);

            if(this.entitySelecterFunction == null) {
                // Call function on all.
                foreach (SidedObjectEntity entity in candidates) {
                    this.mainFunction.Invoke(entity);
                }
            } else {
                SidedObjectEntity e = this.entitySelecterFunction.Invoke(candidates);
                this.mainFunction.Invoke(e);
            }
        }

        /// <summary>
        /// Returns the mask of this button, or if this is a child button, it's parent's mask.
        /// </summary>
        public virtual int getMask() {
            return this.mask;
        }

        /// <summary>
        /// Returns the text to display on the button.  Override to provide fancier text.
        /// </summary>
        public virtual string getText() {
            return this.displayName;
        }

        protected List<SidedObjectEntity> getCandidates<T>(List<T> list) where T : SidedObjectEntity {
            int buttonMask = this.getMask();
            List<SidedObjectEntity> candidates = new List<SidedObjectEntity>(list.Count);

            // Weed out the ones that can't call this action by looking at the mask.
            foreach (SidedObjectEntity entity in list) {
                if ((entity.getButtonMask() & buttonMask) != 0) {
                    candidates.Add(entity);
                }
            }

            return candidates;
        }
    }
}
