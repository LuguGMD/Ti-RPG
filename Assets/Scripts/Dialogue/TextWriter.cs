using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RPG.Dialogue
{
    public class TextWriter
    {
        private readonly char[] textChars;
        private readonly List<(int, string)> tags;

        public TextWriter(string text)
        {
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

            Progress = 0.0f;
        }

        public float TotalLength => textChars.Length;
        public int CurrentLength { get; set; }
        public float Progress
        {
            get => CurrentLength / textChars.Length;
            set => CurrentLength = Mathf.FloorToInt(Mathf.Clamp01(value) * textChars.Length);
        }
        public string CurrentText => ToString();

        public override string ToString()
        {
            StringBuilder textBuilder = new();
            textBuilder.Append(textChars);

            textBuilder.Insert(CurrentLength, "<color=#0000>");
            textBuilder.Append("</color>");

            for (int tagIndex = tags.Count - 1; 0 <= tagIndex; --tagIndex)
            {
                (int tagTextIndex, string tag) = tags[tagIndex];

                if (tagTextIndex <= CurrentLength)
                { textBuilder.Insert(tagTextIndex, tag); }
                else
                { break; }
            }

            return textBuilder.ToString();
        }
    }
}
