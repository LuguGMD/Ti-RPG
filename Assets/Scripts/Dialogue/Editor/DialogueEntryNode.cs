using UnityEngine;
using System;

namespace RPG.Dialogue.Editor
{
    [Serializable]
    public class DialogueEntryNode : DialogueNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueInfo(context);
            context.AddOutputPort(OUTPUT_NEXT_DIALOGUE);
        }
    }
}
