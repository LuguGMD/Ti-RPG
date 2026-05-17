using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG
{
    public class PartyCharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public CharacterScriptable character;

        public PartyManagerUI uiManager;
        public int partyIndex;

        private Vector3 originalScale;
        private float scaleMultiplier = 1.25f;

        private void Start()
        {
            originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = originalScale * scaleMultiplier;
            uiManager.UpdateCharacterInfo(character);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = originalScale;
        }

        public void OnClick()
        {
            uiManager.SelectPartyMember(partyIndex);
        }
    }
}
