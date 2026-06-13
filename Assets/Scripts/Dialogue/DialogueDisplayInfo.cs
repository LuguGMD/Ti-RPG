using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueDisplayInfo
    {
        readonly public string Title;
        readonly public Sprite Sprite;

        public DialogueDisplayInfo(string title, Sprite sprite)
        {
            Title = title;
            Sprite = sprite;
        }
    }
}
