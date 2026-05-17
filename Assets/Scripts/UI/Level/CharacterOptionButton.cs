using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG
{
    public class CharacterOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public PartyManagerUI uiManager;
        public CharacterScriptable character;

        private Vector3 originalScale;
        private float scaleMultiplier = 1.1f;

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
            uiManager.ReplaceCharacter(character);
        }
    }
}
