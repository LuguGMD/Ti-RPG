using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RPG.Dialogue
{
    public static class RichTagProcessor
    {
        public delegate void RichTagHandler(object invoker, string tagContent, string tagArgument);

        private static readonly Dictionary<Type, Dictionary<string, RichTagHandler>> _handlers = new Dictionary<Type, Dictionary<string, RichTagHandler>>();

        static RichTagProcessor()
        {
            // Register default handler for plain text
            RegisterTag(typeof(RichTagProcessor), "", (invoker, tagContent, tagArgument) =>
            {
                Debug.Log(tagArgument);
            });

            RegisterTag(typeof(RichTagProcessor), "color", (invoker, tagContent, tagArgument) =>
            {
                Debug.Log($"Color Tag: Argument = {tagArgument}, Content = {tagContent}");
            });

            RegisterTag(typeof(RichTagProcessor), "b", (invoker, tagContent, tagArgument) =>
            {
                Debug.Log($"Bold Tag: Content = {tagContent}");
            });
        }

        public static void RegisterTag(Type type, string tagContent, RichTagHandler handler)
        {
            if (!_handlers.ContainsKey(type))
            {
                _handlers[type] = new Dictionary<string, RichTagHandler>();
            }

            _handlers[type][tagContent] = handler;
        }
        public static void Process<T>(object invoker, string text) where T : class
        {
            var regex = new Regex(@"<(?<end>/)?(?<tag>\w+)(=(?<arg>[^>]+))?>", RegexOptions.Compiled);

            Stack<(string tag, string arg, int startIndex)> stack = new Stack<(string tag, string arg, int startIndex)>();
            int lastIndex = 0;

            foreach (Match match in regex.Matches(text))
            {
                if (match.Index > lastIndex && stack.Count == 0)
                {
                    string plainText = text.Substring(lastIndex, match.Index - lastIndex);
                    HandlePlainText<T>(invoker, plainText);
                }

                bool isClosing = match.Groups["end"].Success;
                string tag = match.Groups["tag"].Value.ToLower();
                string arg = match.Groups["arg"].Value;

                if (!isClosing)
                {
                    stack.Push((tag, arg, match.Index + match.Length));
                }
                else
                {
                    if (stack.Count <= 0) return;

                    (string tag, string arg, int startIndex) open = stack.Pop();
                    string content = text.Substring(open.startIndex, match.Index - open.startIndex);

                    if (_handlers.TryGetValue(typeof(T), out var typeHandlers))
                    {
                        if (typeHandlers.TryGetValue(open.tag, out RichTagHandler handler))
                        {
                            handler(invoker, content, open.arg);
                        }
                    }
                }

                lastIndex = match.Index + match.Length;
            }

            if (lastIndex < text.Length)
            {
                HandlePlainText<T>(invoker, text.Substring(lastIndex));
            }

        }

        private static void HandlePlainText<T>(object invoker, string text) where T : class
        {
            if (_handlers.TryGetValue(typeof(T), out var typeHandlers))
            {
                if (typeHandlers.TryGetValue("", out RichTagHandler handler))
                {
                    handler(invoker, "", text);
                    return;
                }
            }
        }

    }
}