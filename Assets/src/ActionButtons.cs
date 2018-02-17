using src.button;
using src.util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src {

    public class ActionButtons : MonoBehaviour {

        private const int MAX_BUTTONS = 8;

        // Referenses
        public GameObject buttonPrefab;
        public CameraMover cameraMover;

        private ButtonWrapper[] buttonObjs;
        private ButtonWrapper[] subButtonsObj;
        private bool popupShown = false;

        private Transform subButtonCanvas;

        private List<ActionButton> actionButtons;

        private void Awake() {
            this.subButtonCanvas = this.transform.GetChild(0);
            this.actionButtons = new List<ActionButton>();

            this.buttonObjs = this.makeButtons(this.transform, false);
            this.subButtonsObj = this.makeButtons(this.subButtonCanvas, true);
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
                    this.buttonObjs[i].setText(ab.name);
                    this.buttonObjs[i].button.gameObject.SetActive(true);
                }
                else {
                    this.buttonObjs[i].button.gameObject.SetActive(false);
                }
            }
        }

        private int getMask() {
            int mask = 0;
            if(this.cameraMover.selectedBuilding != null) {
                mask = this.cameraMover.selectedBuilding.getButtonMask();
            }
            else {
                mask = this.cameraMover.party.getPartyMask();
            }
            return mask;
        }

        private void buttonCallback(int index, bool isSub) {
            print(index);
            ActionButton ab = this.actionButtons[index];

            if(!isSub) {
                if (ab is ActionButtonParent) {
                    ActionButtonParent b = (ActionButtonParent)ab;

                    for (int i = 0; i < MAX_BUTTONS; i++) {
                        ButtonWrapper bw = this.buttonObjs[i];
                        bw.button.interactable = this.popupShown || i == index;
                    }

                    // Set button text.
                    ActionButton[] subButtons = b.getSubButtons();
                    for (int i = 0; i < MAX_BUTTONS; i++) {
                        if (i < subButtons.Length && !this.popupShown) {
                            ActionButton ab2 = subButtons[i];
                            this.subButtonsObj[i].setText(ab2.name);
                            this.subButtonsObj[i].button.gameObject.SetActive(true);
                        }
                        else {
                            this.subButtonsObj[i].button.gameObject.SetActive(false);
                        }
                    }

                    // Shift canvas up or down.
                    this.subButtonCanvas.position = new Vector3(this.subButtonCanvas.position.x, this.buttonObjs[index].button.transform.position.y + 24, 0);
                    this.popupShown = !this.popupShown;
                }
                else {
                    this.func(ab);
                }
            } else {
                ActionButtonParent b = (ActionButtonParent)ab;
                this.func(b.getSubButtons()[index]);
            }


            this.updateButtons();
        }

        private void func(ActionButton ab) {
            if (this.cameraMover.selectedBuilding != null) {
                ab.function(this.cameraMover.selectedBuilding);
            }
            else {
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
