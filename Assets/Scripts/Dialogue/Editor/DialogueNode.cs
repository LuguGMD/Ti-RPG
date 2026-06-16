using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEditor;
using RPG.Management;
using System;

namespace RPG.Dialogue.Editor
{
    [Serializable]
    public class DialogueNode : Node
    {
        public const string OUTPUT_NEXT_DIALOGUE = "Next Dialogue";
        public const string INPUT_DIALOGUE_KEY = "Key";
        public const string INPUT_DIALOGUE_DISPLAY_INFO = "Display";
        public const string INPUT_PREVIOUS_DIALOGUE = "Previous Dialogue";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(INPUT_PREVIOUS_DIALOGUE);
            DialogueInfo(context);
            context.AddOutputPort(OUTPUT_NEXT_DIALOGUE);
        }

        protected void DialogueInfo(IPortDefinitionContext context)
        {
            context.AddInputPort<string>(INPUT_DIALOGUE_KEY);
            context.AddInputPort<EntityScriptable>(INPUT_DIALOGUE_DISPLAY_INFO);
        }
    }
}
