using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace RPG.Management.Progression.Editor
{
    [Serializable]
    public class UpgradeGraphNode : Node
    {
        public const string PARENT_COUNT_NAME = "ParentCount";
        public const string INPUT_NAME = "Name";
        public const string INPUT_DESCRIPTION = "Description";
        public const string INPUT_PRICE = "Price";
        public const string INPUT_ICON = "Icon";
        public const string INPUT_PARENT = "Parent ";
        public const string OUTPUT_CHILDS = "Output";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<int>(PARENT_COUNT_NAME).WithDisplayName("Parent Count").WithDefaultValue(1).Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            var parentCountOption = GetNodeOptionByName(PARENT_COUNT_NAME);
            parentCountOption.TryGetValue<int>(out int parentCount);

            context.AddInputPort<string>(INPUT_NAME).Build();
            context.AddInputPort<string>(INPUT_DESCRIPTION).Build();
            context.AddInputPort<int>(INPUT_PRICE).Build();

            context.AddInputPort<Sprite>(INPUT_ICON).Build();

            for (int i = 0; i < parentCount; i++)
            {
                context.AddInputPort(INPUT_PARENT + i).Build();
            }

            context.AddOutputPort(OUTPUT_CHILDS).Build();
        }
    }
}
