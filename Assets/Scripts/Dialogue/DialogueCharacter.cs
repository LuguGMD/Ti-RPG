using RPG.Management.Interaction;
using UnityEngine;
using UnityEngine.Localization;

namespace RPG.Dialogue
{
    public class DialogueCharacter : MonoBehaviour, IInteractable
    {
        [SerializeField] private LocalizedStringTable _stringTable;
        [SerializeField] private string _entryDialogueKey;

        #region Properties

        public LocalizedStringTable StringTable { get { return _stringTable; } }
        public string EntryDialogueKey { get { return _entryDialogueKey; } }

        #endregion

        public void Interact()
        {
            DialogueManager.Instance.SetDialogueCharacter(this);
        }
    }
}
