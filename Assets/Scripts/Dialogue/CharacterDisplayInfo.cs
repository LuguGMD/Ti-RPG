using RPG.Combat;

namespace RPG.Dialogue
{
    public class CharacterDisplayInfo : DialogueDisplayInfo
    {
        readonly public CharacterScriptable Character;
        public CharacterDisplayInfo(CharacterScriptable character)
        : base(character.EntityName, character.Icon)
        {
            Character = character;
        }
    }
}
