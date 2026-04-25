using System;
using Unity.GraphToolkit.Editor;
namespace QuestMaker.Editor.Nodes
{
    internal class QMStartingNode : QMBaseNode
    {
        public const string QUEST_ID_PORT = "Quest_id";
        public const string QUEST_NAME_PORT = "Quest_name";

        public override Type PortType => GetType();

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(QUEST_ID_PORT, typeof(string))
                .WithDisplayName("Quest ID")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The id of the quest")
                .Build();

            context.AddOption(QUEST_NAME_PORT, typeof(string))
                .WithDisplayName("Quest name")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The name of the quest")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(OUTPUT_PORT)
                .WithDataType(typeof(QMFlowHelper))
                .WithDisplayName(OUTPUT_PORT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}
