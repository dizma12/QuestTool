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
        public const string QUEST_DESC_PORT = "Quest_id";
        public const string QUEST_NAME_PORT = "Quest_name";

        public const string SPECIAL_EVENT_PORT = "Special_Event_Port";
        public const string OBJECTIVE_FLOW_PORT = "Objective_Flow_Port";
        public const string CONTEXT_FLOW_PORT = "Context_Flow_Port";
        public override Type PortType => typeof(IContextFlowHelper);
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(QUEST_NAME_PORT, typeof(string))
                .WithDisplayName("Name")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The name of the quest")
                .Build();

            context.AddOption(QUEST_DESC_PORT, typeof(string))
                .WithDisplayName("Description")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The Description of the quest")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(SPECIAL_EVENT_PORT)
                .WithDataType(typeof(ISpecialEventNode))
                .WithDisplayName("Special Event")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();

            context.AddOutputPort(CONTEXT_FLOW_PORT)
                .WithDataType(PortType)
                .WithDisplayName("Context")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort(OBJECTIVE_FLOW_PORT)
                .WithDataType(typeof(IObjectiveFlowHelper))
                .WithDisplayName("OBjectives")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
        public virtual void ProcessNode(ModuleBuilderRegistry reg)
        {
            IQuestInfoModule module = reg.RequestModule<QuestInfoModule, IQuestInfoModule>();

            if (module == null) return;

            INodeOption option = GetNodeOptionByName(QUEST_NAME_PORT);

            if (option.TryGetValue(out string name))
                module.SetQuestName(name);

            option = GetNodeOptionByName(QUEST_DESC_PORT);

            if (option.TryGetValue(out string desc))
                module.SetQuestDescription(desc);
        }
    }
}
