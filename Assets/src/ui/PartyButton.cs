using UnityEngine;
using UnityEngine.EventSystems;

namespace src.ui {

    public class PartyButton : MonoBehaviour, IPointerClickHandler {

        private SelectedParty party;
        private int index;

        private void Awake() {
            this.party = this.GetComponentInParent<SelectedParty>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                this.party.onLeftClick(this.index);
            } else if (eventData.button == PointerEventData.InputButton.Middle) {
                
            } else if (eventData.button == PointerEventData.InputButton.Right) {
                this.party.onRightClick(this.index);
            }
        }

        public void setIndex(int i) {
            this.index = i;
        }
    }
}
