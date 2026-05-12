using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class TextProgress
    {
        private readonly char[] textChars;
        private readonly List<(int, string)> tags;

        public float Progress { get; private set; }

        public TextProgress(string text)
        {
            Progress = 0;

            textChars = new char[text.Length];
            tags = new();

            StringBuilder tagBuffer = null;
            int parsedIndex = 0;
            for (int textIndex = 0; textIndex < text.Length; textIndex++)
            {
                char currentChar = text[textIndex];
                if (currentChar == '<')
                {
                    tagBuffer = new();
                }

                if (tagBuffer != null)
                {
                    tagBuffer.Append(currentChar);

                    if (currentChar == '>')
                    {
                        tags.Add((parsedIndex, tagBuffer.ToString()));
                        tagBuffer = null;
                        parsedIndex++;
                    }
                }
                else
                {
                    textChars[parsedIndex] = currentChar;
                    parsedIndex++;
                }
            }
            if (tagBuffer != null)
            {
                tagBuffer.CopyTo(0, textChars, parsedIndex, tagBuffer.Length);
                parsedIndex += tagBuffer.Length;
            }

            if (parsedIndex < textChars.Length)
            { Array.Resize(ref textChars, parsedIndex); }
        }

        public void SetProgress(float progress)
        {
            this.Progress = progress;
        }
        public void SetProgress(int chars)
        {
            Progress = (float)chars / textChars.Length;
        }

        public override string ToString()
        {
            int charShownCount = Mathf.RoundToInt(Mathf.Clamp01(Progress) * textChars.Length);

            StringBuilder textBuilder = new();
            textBuilder.Append(textChars);

            textBuilder.Insert(charShownCount, "<color=#0000>");
            textBuilder.Append("</color>");

            for (int tagIndex = tags.Count - 1; 0 <= tagIndex; --tagIndex)
            {
                (int tagTextIndex, string tag) = tags[tagIndex];

                if (tagTextIndex <= charShownCount)
                { textBuilder.Insert(tagTextIndex, tag); }
                else
                { break; }
            }

            return textBuilder.ToString();
        }
    }
}
