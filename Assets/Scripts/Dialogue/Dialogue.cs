using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization.Tables;

namespace RPG
{
    public class Dialogue
    {
        private readonly StringTableEntry dialogue;

        public Dialogue(StringTableEntry dialogue)
        {
            this.dialogue = dialogue;
            ParseEntry();
        }
        static public Dialogue Get(StringTable table, string entryKey)
        {
            StringTableEntry entry = table[entryKey];

            string[] keySplit = entryKey.Split('_');
            string keyCurrent = keySplit[^1];
            if (keyCurrent == "CHOICE")
            { return new WithChoice(entry); }
            else
            { return new Dialogue(entry); }
        }

        private StringTable Table => dialogue.Table as StringTable;

        public string Key => dialogue.Key;
        public string SpriteKey { get; private set; }
        public string TextLocalized { get; private set; }

        private void ParseEntry()
        {
            string textLocalizedRaw = dialogue.LocalizedValue;

            string spriteKey = null;
            string textLocalized = null;
            for (int charIndex = 0; charIndex < textLocalizedRaw.Length; charIndex++)
            {
                if (textLocalizedRaw[charIndex] == ' ')
                { break; }

                if (textLocalizedRaw[charIndex] == '\n')
                {
                    string[] parsedText = textLocalizedRaw.Split('\n', 2);
                    spriteKey = parsedText[1];
                    textLocalized = parsedText[2];
                    break;
                }
            }
            if (spriteKey == null)
            { textLocalized = textLocalizedRaw; }

            SpriteKey = spriteKey;
            TextLocalized = textLocalized;
        }

        public virtual Dialogue GetNext()
        {
            string[] keySplit = Key.Split('_');

            string keyCurrent = keySplit[^1];
            if (keyCurrent == "LAST")
            { return null; }

            string keyBody = string.Join('_', keySplit[..^1]);

            int keyCurrentId = Convert.ToInt32(keyCurrent);
            string nextKey = $"{keyBody}_{keyCurrentId + 1}";
            StringTableEntry nextEntry = Table.GetEntry(nextKey);

            if (nextEntry == null)
            {
                string nextKeyAsChoice = $"{nextKey}_CHOICE";
                nextEntry = Table[nextKeyAsChoice];
                if (nextEntry != null)
                { return new WithChoice(nextEntry); }

                string nextKeyAsLast = $"{nextKey}_LAST";
                nextEntry = Table[nextKeyAsLast];
                if (nextEntry != null)
                { return new Dialogue(nextEntry); }

                else
                {
                    Debug.LogWarning(
                        $"{Table.name}: {Key}\n" +
                        $"O próximo diálogo não foi encontrado ( {nextKey} ). " +
                        $"Se for intencionalmente o último diálogo, indique na chave. " +
                        $"( {nextKeyAsLast} ).\n"
                    );
                    return null;
                }
            }

            Dialogue nextDialogue = new(nextEntry);
            return nextDialogue;
        }

        public class WithChoice : Dialogue
        {
            private readonly List<Choice> choices;
            public IReadOnlyList<Choice> Choices => choices;
            private Choice Selected { get; set; }

            public WithChoice(StringTableEntry dialogue) : base(dialogue)
            {
                choices = new();

                string keyBody = Key;

                int keyCurrentId = 1;
                string keyCurrent;
                do
                {
                    keyCurrent = keyCurrentId.ToString();
                    string nextKey = $"{keyBody}_{keyCurrent}";
                    StringTableEntry nextEntry = Table.GetEntry(nextKey);

                    if (nextEntry != null)
                    {
                        Choice choice = new(nextEntry, keyCurrentId);
                        choices.Add(choice);
                        keyCurrentId++;
                    }
                    else
                    {
                        keyCurrent = "LAST";
                        string nextKeyAsLast = $"{nextKey}_{keyCurrent}";
                        nextEntry = Table.GetEntry(nextKeyAsLast);
                        if (nextEntry != null)
                        {
                            Choice choice = new(nextEntry, keyCurrentId);
                            choices.Add(choice);
                        }

                        else
                        {
                            Debug.LogWarning(
                                $"{Table.name}: {Key}\n" +
                                $"A última escolha não foi encontrada ( {nextKey} ). " +
                                $"Se for intencionalmente a última escolha, indique na chave. " +
                                $"( {nextKeyAsLast} ).\n"
                            );
                        }
                    }
                }
                while (keyCurrent != "LAST");

                if (choices.Count == 0)
                {
                    Debug.LogWarning(
                        $"{Table.name}: {Key}\n" +
                        $"Nenhuma escolha foi encontrada. " +
                        $"Adicione escolhas indicando seu índice ( {Key}_1, {Key}_2... ).\n"
                    );
                }
                else
                {
                    Select(1);
                }
            }

            public void Select(int id)
            {
                Selected = choices[id - 1];
            }

            public override Dialogue GetNext() => Selected.GetNext();
        }

        public class Choice : Dialogue
        {
            private int keyCurrentId;
            public Choice(StringTableEntry dialogue, int keyCurrentId) : base(dialogue)
            {
                this.keyCurrentId = keyCurrentId;
            }

            public override Dialogue GetNext()
            {
                string[] keySplit = Key.Split('_');

                string keyCurrent = keySplit[^1];
                string keyBody = keyCurrent != "LAST" ?
                    string.Join('_', keySplit[..^4])
                  : string.Join('_', keySplit[..^5]);

                string nextKey = keyBody.Length == 0 ?
                    $"CHOICE_{keyCurrentId}_LINE_1"
                  : $"{keyBody}_CHOICE_{keyCurrentId}_LINE_1";

                StringTableEntry nextEntry = Table[nextKey];
                if (nextEntry != null)
                { return new Dialogue(nextEntry); }

                else
                {
                    string nextKeyAsLast = $"{nextKey}_LAST";
                    nextEntry = Table[nextKeyAsLast];
                    if (nextEntry != null)
                    { return new Dialogue(nextEntry); }

                    else
                    {
                        Debug.LogWarning(
                            $"{Table.name}: {Key}\n" +
                            $"O próximo diálogo não foi encontrado ( {nextKey} ). " +
                            $"Se for intencionalmente o último diálogo, indique na chave. " +
                            $"( {nextKeyAsLast} ).\n"
                        );
                        return null;
                    }

                    // LINE_1
                    // LINE_2_CHOICE
                    // LINE_2_CHOICE_1
                    // > CHOICE_1_LINE_1
                    // > CHOICE_1_LINE_1
                    // LINE_2_CHOICE_2_LAST
                    // > CHOICE_2_LINE_1
                    // > CHOICE_2_LINE_2_CHOICE
                    // > CHOICE_2_LINE_2_CHOICE_1
                    //   > CHOICE_2_CHOICE_1_LINE_1
                    //   > CHOICE_2_CHOICE_1_LINE_2
                    // > CHOICE_2_LINE_2_CHOICE_2_LAST
                    //   > CHOICE_2_CHOICE_2_LINE_1
                    //   > CHOICE_2_CHOICE_2_LINE_2
                }
            }
        }
    }
}
