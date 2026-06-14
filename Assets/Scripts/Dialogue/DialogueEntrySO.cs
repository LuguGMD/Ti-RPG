using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueEntry", menuName = "Scriptable Objects/Dialogue/DialogueEntry")]
    public class DialogueEntrySO : ScriptableObject
    {
        [SerializeField] private Dialogue dialogue;
        public Dialogue Dialogue => dialogue;
    }
}
