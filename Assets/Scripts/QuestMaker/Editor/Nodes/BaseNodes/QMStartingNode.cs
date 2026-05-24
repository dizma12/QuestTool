using QuestMaker.Data;
using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using Unity.GraphToolkit.Editor;
namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMStartingNode : QMBaseNode
    {
        public const string QUEST_ID_PORT = "Quest_id";
        public const string QUEST_NAME_PORT = "Quest_name";

        public const string SPECIAL_EVENT_PORT = "OnStart";


        public override Type PortType => typeof(QMFlowHelper);

        public void Build(QuestSO quest)
        {
            INodeOption option = GetNodeOptionByName(QUEST_ID_PORT);
            option.TryGetValue(out string value);
            quest.QuestID = value;

            option = GetNodeOptionByName(QUEST_NAME_PORT);
            option.TryGetValue(out value);
            quest.QuestName = value;
        }

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
            context.AddOutputPort(SPECIAL_EVENT_PORT)
                .WithDataType(typeof(ISpecialEventNode))
                .WithDisplayName("Special Event")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();

            context.AddOutputPort(OUTPUT_PORT)
                .WithDataType(typeof(QMFlowHelper))
                .WithDisplayName(OUTPUT_PORT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

    }
}
