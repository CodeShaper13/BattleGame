using codeshaper.buildings;
using codeshaper.button;
using codeshaper.team;
using codeshaper.util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace codeshaper {

    public class ActionButtons : MonoBehaviour {

        private const int MAX_BUTTONS = 8;

        public GameObject buttonPrefab;

        private CameraMover cameraMover;
        private ButtonWrapper[] buttonObjs;
        private ButtonWrapper[] subButtonsObj;
        /// <summary> If true, the popup buttons are shown. </summary>
        private bool popupShown = false;

        private Transform subButtonCanvas;

        /// <summary> A list of the current buttons that are shown on the side of the screen.  This does NOT contain the sub buttons. </summary>
        private List<ActionButton> currentlyShownButtons;
        /// <summary> Saves what button was clicked when dealing with sub buttons. </summary>
        private int selectedMainButtonIndex = -1;

        /// <summary> Stores </summary>
        private bool flag;

        private void Awake() {
            this.cameraMover = this.GetComponentInParent<CameraMover>();
            
            this.subButtonCanvas = this.transform.GetChild(0);
            this.currentlyShownButtons = new List<ActionButton>();

            this.buttonObjs = this.makeButtons(this.transform, false);
            this.subButtonsObj = this.makeButtons(this.subButtonCanvas, true);
        }

        private void Update() {
            if(this.popupShown) {
                foreach (ActionButton ab in this.currentlyShownButtons) {
                    if (ab is ActionButtonParent) {
                        ActionButtonParent abp = (ActionButtonParent)ab;
                        Team team = this.cameraMover.getControllingTeam();

                        ActionButton[] abArray = abp.getSubButtons();
                        for (int i = 0; i < abArray.Length; i++) {
                            ActionButton childButton = abArray[i];

                            bool flag;
                            if (abp.childUpdateAction != null) {
                                flag = abp.childUpdateAction(childButton, team);
                            } else {
                                flag = true;
                            }
                            this.subButtonsObj[i].setInteractable(flag);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Shows the correct buttons on the side of the screen based on the selected units or building.
        /// </summary>
        public void updateSideButtons() {
            int mask = this.getMask();
            this.currentlyShownButtons.Clear();

            for (int i = 0; i < ActionButton.buttonList.Length; i++) {
                ActionButton button = ActionButton.buttonList[i];
                if (button != null && ((mask >> i) & 1) == 1) {
                    this.currentlyShownButtons.Add(button);
                }
            }

            for(int i = 0; i < MAX_BUTTONS; i++) {
                if(i < this.currentlyShownButtons.Count) {
                    ActionButton ab = this.currentlyShownButtons[i];
                    this.buttonObjs[i].setText(ab.getText());
                    this.buttonObjs[i].setVisible(true);
                }
                else {
                    this.buttonObjs[i].setVisible(false);
                }
            }
        }

        /// <summary>
        /// Closes the popup buttons and makes it so you can interacted with the main buttons.
        /// </summary>
        public void closePopupButtons() {
            for (int i = 0; i < MAX_BUTTONS; i++) {
                // Hide sub buttons.
                this.subButtonsObj[i].setVisible(false);

                // Enable the main buttons.
                this.buttonObjs[i].setInteractable(true);
            }

            this.popupShown = false;
        }

        /// <summary>
        /// Returns the button bit mask to use based on the selected object(s).
        /// </summary>
        private int getMask() {
            BuildingBase building = this.cameraMover.selectedBuilding.getBuilding();
            if (building != null) {
                return building.getButtonMask();
            } else {
                return this.cameraMover.party.getPartyMask();
            }
        }

        /// <summary>
        /// Called every time a button is clicked.
        /// </summary>
        /// <param name="index"> The index of the clicked button. </param>
        /// <param name="isSubButton"> True if it is a sub button. </param>
        private void buttonCallback(int index, bool isSubButton) {
            if(!isSubButton) {
                // If a main button was clicked...
                ActionButton clickedButton = this.currentlyShownButtons[index];
                if (clickedButton is ActionButtonParent) {
                    // Clicked on a parent button conatining sub buttons.

                    // Disable all the main buttons except for the selected one.
                    for (int i = 0; i < MAX_BUTTONS; i++) {
                        this.buttonObjs[i].setInteractable(this.popupShown || i == index);
                    }

                    // Set the sub button text and set them to be active.
                    ActionButton[] subButtons = ((ActionButtonParent)clickedButton).getSubButtons();
                    for (int i = 0; i < MAX_BUTTONS; i++) {
                        if (i < subButtons.Length && !this.popupShown) {
                            this.subButtonsObj[i].setText(subButtons[i].getText());
                            this.subButtonsObj[i].setVisible(true);
                        }
                        else {
                            this.subButtonsObj[i].setVisible(false);
                        }
                    }

                    // Shift the sub button canvas up or down.
                    this.subButtonCanvas.position = new Vector3(this.subButtonCanvas.position.x, this.buttonObjs[index].button.transform.position.y + 24, 0);
                    this.popupShown = !this.popupShown;

                    this.selectedMainButtonIndex = index;
                }
                else {
                    // Clicked on a normal button.
                    this.callFunctionOnSelected(clickedButton);
                }
            } else {
                // If a sub button was clicked...
                ActionButton[] subButtons = ((ActionButtonParent)this.currentlyShownButtons[this.selectedMainButtonIndex]).getSubButtons();
                this.closePopupButtons();                
                this.callFunctionOnSelected(subButtons[index]);
            }

            this.updateSideButtons();
        }

        /// <summary>
        /// Calls the function of the passed ActionButton on whatever is selected (troop or building).
        /// </summary>
        private void callFunctionOnSelected(ActionButton ab) {
            if (this.cameraMover.selectedBuilding.getBuilding() != null) {
                ab.function(this.cameraMover.selectedBuilding.getBuilding());
            } else {
                this.cameraMover.party.callOnAll(ab);
            }
        }

        /// <summary>
        /// Used by the constructor to make a list of buttons.  This list is then returned.
        /// </summary>
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
