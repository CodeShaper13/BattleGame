using src.button;
using src.util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src {

    public class ActionButtons : MonoBehaviour {

        private const int MAX_BUTTONS = 8;

        public GameObject buttonPrefab;

        private CameraMover cameraMover;
        private ButtonWrapper[] buttonObjs;
        private ButtonWrapper[] subButtonsObj;
        private bool popupShown = false;

        private Transform subButtonCanvas;

        private List<ActionButton> actionButtons;
        /// <summary> Saves what button was clicked when dealing with sub buttons. </summary>
        private int selectedMainButtonIndex = -1;

        /// <summary> Stores </summary>
        private bool flag;

        private void Awake() {
            this.cameraMover = this.GetComponentInParent<CameraMover>();
            
            this.subButtonCanvas = this.transform.GetChild(0);
            this.actionButtons = new List<ActionButton>();

            this.buttonObjs = this.makeButtons(this.transform, false);
            this.subButtonsObj = this.makeButtons(this.subButtonCanvas, true);
        }

        private void Update() {
            //TODO flag
        }

        public void updateButtons() {
            int mask = this.getMask();
            this.actionButtons.Clear();

            for (int i = 0; i < ActionButton.buttonList.Length; i++) {
                ActionButton button = ActionButton.buttonList[i];
                if (button != null && ((mask >> i) & 1) == 1) {
                    this.actionButtons.Add(button);
                }
            }

            for(int i = 0; i < MAX_BUTTONS; i++) {
                if(i < this.actionButtons.Count) {
                    ActionButton ab = this.actionButtons[i];
                    this.buttonObjs[i].setText(ab.getText());
                    this.buttonObjs[i].setActive(true);
                }
                else {
                    this.buttonObjs[i].setActive(false);
                }
            }
        }

        public void closePopupButtons() {
            for (int i = 0; i < MAX_BUTTONS; i++) {
                // Enable the main buttons.
                this.buttonObjs[i].button.interactable = true;

                // Hide sub buttons.
                this.subButtonsObj[i].setActive(false);
            }

            this.popupShown = false;
        }

        /// <summary>
        /// Returns the button bit mask to use based on the selected object(s).
        /// </summary>
        private int getMask() {
            if(this.cameraMover.selectedBuilding != null) {
                return this.cameraMover.selectedBuilding.getButtonMask();
            } else {
                return this.cameraMover.party.getPartyMask();
            }
        }

        private void buttonCallback(int index, bool isSubButton) {
            if(!isSubButton) {
                ActionButton ab = this.actionButtons[index];
                if (ab is ActionButtonParent) {
                    // Clicked on a parent button conatining sub buttons.
                    ActionButtonParent b = (ActionButtonParent)ab;

                    // Disable all the main buttons except for the selected one.
                    for (int i = 0; i < MAX_BUTTONS; i++) {
                        this.buttonObjs[i].button.interactable = (this.popupShown || i == index);
                    }

                    // Set the sub button text and set them to be active.
                    ActionButton[] subButtons = b.getSubButtons();
                    for (int i = 0; i < MAX_BUTTONS; i++) {
                        if (i < subButtons.Length && !this.popupShown) {
                            this.subButtonsObj[i].setText(subButtons[i].getText());
                            this.subButtonsObj[i].setActive(true);
                        }
                        else {
                            this.subButtonsObj[i].setActive(false);
                        }
                    }

                    // Shift the sub button canvas up or down.
                    this.subButtonCanvas.position = new Vector3(this.subButtonCanvas.position.x, this.buttonObjs[index].button.transform.position.y + 24, 0);
                    this.popupShown = !this.popupShown;

                    this.selectedMainButtonIndex = index;
                }
                else {
                    // Clicked on a normal button.
                    this.callFunctionOnSelected(ab);
                }
            } else {
                // Clicked on a sub button.
                ActionButton[] subButtons = ((ActionButtonParent)this.actionButtons[this.selectedMainButtonIndex]).getSubButtons();
                this.closePopupButtons();                
                this.callFunctionOnSelected(subButtons[index]);
            }

            this.updateButtons();
        }

        /// <summary>
        /// Calls the function of the passed ActionButton on whatever it is that is selected (troop or building).
        /// </summary>
        private void callFunctionOnSelected(ActionButton ab) {
            if (this.cameraMover.selectedBuilding != null) {
                ab.function(this.cameraMover.selectedBuilding);
            } else {
                this.cameraMover.party.callOnAll(ab.function);
            }
        }

        private ButtonWrapper[] makeButtons(Transform parent, bool isSub) {
            ButtonWrapper[] buttonList = new ButtonWrapper[MAX_BUTTONS];

            for (int i = 0; i < MAX_BUTTONS; i++) {
                GameObject btn = GameObject.Instantiate(this.buttonPrefab, parent);
                btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(80, -((48 * i) + 24), 0);
                btn.name = (isSub ? "Sub" : "Action") + "Button[" + i + "]";

                Button b = btn.GetComponent<Button>();
                int j = i;
                bool flag = isSub;
                b.onClick.AddListener(() => { this.buttonCallback(j, flag); });

                btn.SetActive(false);
                buttonList[i] = new ButtonWrapper(btn.GetComponent<Button>());
            }
            return buttonList;
        }
    }
}
