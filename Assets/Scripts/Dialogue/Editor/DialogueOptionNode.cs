using Unity.GraphToolkit.Editor;
using UnityEngine;
using System;

namespace RPG.Dialogue.Editor
{
    [Serializable]
    public class DialogueOptionNode : DialogueNode
    {
        public const string OPTIONS_COUNT = "Options Count";
        public const string OUTPUT_OPTION = "Option ";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            base.OnDefineOptions(context);

            context.AddOption<int>(OPTIONS_COUNT).WithDefaultValue(1);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(INPUT_PREVIOUS_DIALOGUE);

            DialogueInfo(context);

            INodeOption option = GetNodeOptionByName(OPTIONS_COUNT);
            option.TryGetValue<int>(out int optionsCount);

            for (int i = 0; i < optionsCount; i++) 
            {
                context.AddOutputPort(OUTPUT_OPTION + i);
            }
        }

        
    }
}
