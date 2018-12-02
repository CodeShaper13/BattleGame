using codeshaper.entity.unit;
using codeshaper.ui;
using System.Collections.Generic;
using UnityEngine;
using codeshaper.util.outline;
using codeshaper.button;

namespace codeshaper.selected {

    public class SelectedParty : SelectedDisplayerBase {

        private const int PARTY_SIZE = 12;

        public GameObject buttonPrefab;

        [SerializeField] // For debugging in inspector
        private List<UnitBase> units;
        private CameraMover cameraMover;
        private PartyButton[] partyButtons;

        protected override void Awake() {
            base.Awake();

            this.units = new List<UnitBase>();
            this.partyButtons = new PartyButton[PARTY_SIZE];

            for(int i = 0; i < PARTY_SIZE; i++) {
                GameObject btn = GameObject.Instantiate(this.buttonPrefab, this.transform);
                RectTransform rt = btn.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector3(-((rt.sizeDelta.y * i) + 36), 36, 0);
                btn.name = "PartyButton[" + i + "]";

                PartyButton pb = btn.GetComponent<PartyButton>();
                pb.setIndex(i);

                this.partyButtons[i] = pb;
            }
        }

        public void setCameraMover(CameraMover c) {
            this.cameraMover = c;
        }

        public override int getMask() {
            int mask = 0;
            foreach(UnitBase unit in this.units) {
                mask |= unit.getButtonMask();
            }
            return mask;
        }

        public override void callFunctionOn(ActionButton actionButton) {
            actionButton.callFunction(this.getAllUnits());
        }

        public override void clearSelected() {
            foreach (UnitBase unit in this.units) {
                unit.setOutlineVisibility(false, EnumOutlineParam.SELECTED);
            }
            this.units.Clear();
            foreach (PartyButton pb in this.partyButtons) {
                pb.setUnit(null);
            }
            this.hideIfEmpty();
        }

        /// <summary>
        /// Moves all members of the party to the same point.
        /// </summary>
        public void moveAllTo(Vector3 point) {
            foreach (UnitBase u in this.units) {
                u.walkToPoint(point, this.getPartySize());
            }
        }

        /// <summary>
        /// Returns the Unit at the passed index, or null if the index is out of bounds.
        /// </summary>
        public UnitBase getUnit(int index) {
            if(index < this.units.Count) {
                return this.units[index];
            } else {
                return null;
            }
        }

        /// <summary>
        /// Returns the number of units in the party.
        /// </summary>
        public int getPartySize() {
            return this.units.Count;
        }

        /// <summary>
        /// Returns true if the party is full and it can't hold any more members.
        /// </summary>
        public bool isFull() {
            return this.getPartySize() >= PARTY_SIZE;
        }

        /// <summary>
        /// Removes the passed unit from the party if it is in the party.
        /// </summary>
        public void remove(UnitBase unit) {
            int index = this.units.IndexOf(unit);
            if(index != -1) {
                this.partyButtons[index].setUnit(null);
                unit.setOutlineVisibility(false, EnumOutlineParam.SELECTED);
                this.units.RemoveAt(index);

                // Slide the units down so there isn't an empty spot.
                for(int i = index; i < PARTY_SIZE; i++) {
                    this.partyButtons[i].setUnit(this.getUnit(i));
                }
            }

            this.hideIfEmpty();
        }

        public List<UnitBase> getAllUnits() {
            return this.units;
        }

        /// <summary>
        /// Tries to add a Unit to the party.
        /// Returns false if the party is full and the Unit cant be added.
        /// </summary>
        public bool tryAdd(UnitBase unit) {
            if(!this.isFull() && !this.units.Contains(unit)) {
                this.units.Add(unit);
                unit.setOutlineVisibility(true, EnumOutlineParam.SELECTED);
                this.partyButtons[this.units.Count - 1].setUnit(unit);

                this.hideIfEmpty();

                return true;
            } else {
                return false;
            }
        }

        public void onButtonLeftClick(int index) {
            UnitBase unit = this.getUnit(index);
            if (unit != null) {
                this.cameraMover.centerOn(unit.transform.position);
            }
        }

        public void onButtonRightClick(int index) {
            UnitBase unit = this.getUnit(index);
            this.remove(unit);
            this.cameraMover.actionButtons.updateSideButtons();
        }

        /// <summary>
        /// Returns true if the passed unit is a memeber of this party.
        /// </summary>
        public bool contains(UnitBase unit) {
            foreach(UnitBase u in this.units) {
                if(u == unit) {
                    return true;
                }
            }
            return false;
        }

        private void buttonCallback(int index) {
            UnitBase unit = this.getUnit(index);
            if (unit != null) {
                this.cameraMover.centerOn(unit.transform.position);
            }
        }

        /// <summary>
        /// Hides the hud if the party is empty, or reveals it if it has as least one member.
        /// </summary>
        private void hideIfEmpty() {
            this.setUIVisible(this.units.Count != 0);
        }
    }
}
