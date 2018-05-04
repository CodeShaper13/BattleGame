using src.troop;
using src.ui;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace src {

    public class SelectedParty : MonoBehaviour {

        private const int PARTY_SIZE = 8;

        public GameObject buttonPrefab;

        [SerializeField] // For debugging in inspector?
        private List<UnitBase> units;
        private CameraMover cameraMover;
        private Text[] buttonText;

        private void Awake() {
            this.units = new List<UnitBase>();
            this.buttonText = new Text[PARTY_SIZE];

            for(int i = 0; i < PARTY_SIZE; i++) {
                GameObject btn = GameObject.Instantiate(this.buttonPrefab, this.transform);
                btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(-((72 * i) + 36), 36, 0);
                btn.name = "PartyButton[" + i + "]";

                PartyButton pb = btn.GetComponent<PartyButton>();
                pb.setIndex(i);

                //Button b = btn.GetComponent<Button>();
                //int j = i;
                //b.onClick.AddListener(() => { this.buttonCallback(j); });

                this.buttonText[i] = btn.GetComponentInChildren<Text>();
            }

            // Sets button text.
            this.disband();
        }

        private void Update() {
            for (int i = 0; i < PARTY_SIZE; i++) {
                UnitBase unit = this.get(i);
                if(unit != null) {
                    string s = unit.getHealth() + "/" + unit.getMaxHealth();
                    this.buttonText[i].text = unit.getUnitName() + "\n" + s;
                } else {
                    this.buttonText[i].text = "...";
                }
            }
        }

        public void setCameraMover(CameraMover c) {
            this.cameraMover = c;
        }

        public int getPartyMask() {
            if(this.units.Count == 0) {
                return 0;
            }

            int mask = int.MaxValue;
            foreach(UnitBase unit in this.units) {
                mask &= unit.getButtonMask();
            }
            return mask;
        }

        public void moveAllTo(Vector3 point) {
            foreach (UnitBase u in this.units) {
                u.setDestination(point);
            }
        }

        /// <summary>
        /// Returns the Unit at the passed index, or null if the index is out of bounds.
        /// </summary>
        public UnitBase get(int index) {
            if(index < this.units.Count) {
                return this.units[index];
            } else {
                return null;
            }
        }

        public void callOnAll(Action<SidedObjectEntity> action) {
            for (int i = this.units.Count - 1; i >= 0; i--) {
                UnitBase unit = this.units[i];
                action(unit);
            }
        }

        /// <summary>
        /// Returns true if the party is full and it can't hold any more members.
        /// </summary>
        public bool isFull() {
            return this.units.Count >= PARTY_SIZE;
        }

        /// <summary>
        /// Removes the passed unit from the party.
        /// </summary>
        public void remove(UnitBase unit) {
            this.units.Remove(unit);
        }

        /// <summary>
        /// Removes all Units from the party.
        /// </summary>
        public void disband() {
            this.units.Clear();
        }

        /// <summary>
        /// Tries to add a Unit to the party.
        /// Returns false if the party is full and the Unit cant be added.
        /// </summary>
        public bool tryAdd(UnitBase unit) {
            if(!this.isFull() && !this.units.Contains(unit)) {
                this.units.Add(unit);
                return true;
            } else {
                return false;
            }
        }

        public void onLeftClick(int index) {
            UnitBase unit = this.get(index);
            if (unit != null) {
                this.cameraMover.centerOn(unit.transform.position);
            }
        }

        public void onRightClick(int index) {
            UnitBase unit = this.get(index);
            this.remove(unit);
            this.cameraMover.actionButtons.updateButtons();
        }

        private void buttonCallback(int index) {
            UnitBase unit = this.get(index);
            if (unit != null) {
                this.cameraMover.centerOn(unit.transform.position);
            }
        }
    }
}
