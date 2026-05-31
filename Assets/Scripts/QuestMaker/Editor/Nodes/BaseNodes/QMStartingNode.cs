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
        public const string QUEST_DESC_OPTION = "QUEST_DESC_OPTION";
        public const string QUEST_NAME_OPTION = "QUEST_NAME_OPTION";
        public const string QUEST_TYPE_OPTION = "QUEST_TYPE_OPTION";

        public const string SPECIAL_EVENT_PORT = "SPECIAL_EVENT_PORT";
        public const string OBJECTIVE_FLOW_PORT = "OBJECTIVE_FLOW_PORT";
        public const string CONTEXT_FLOW_PORT = "CONTEXT_FLOW_PORT";
        public override Type PortType => typeof(IContextFlowHelper);
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            //Type
            context.AddOption(QUEST_TYPE_OPTION, typeof(QuestType))
                .WithDisplayName("Quest Type")
                .WithDefaultValue(QuestType.Main)
                .WithTooltip("The type of the quest can be used to categorize the quest on runtime.")
                .Build();
            //Name
            context.AddOption(QUEST_NAME_OPTION, typeof(string))
                .WithDisplayName("Name")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The name of the quest")
                .Build();
            //Desc
            context.AddOption(QUEST_DESC_OPTION, typeof(string))
                .WithDisplayName("Description")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The Description of the quest")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            //Event
            context.AddOutputPort(SPECIAL_EVENT_PORT)
                .WithDataType(typeof(ISpecialEventNode))
                .WithDisplayName("Special Event")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
            //Flow
            context.AddOutputPort(CONTEXT_FLOW_PORT)
                .WithDataType(PortType)
                .WithDisplayName("Context")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            //Objective
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

            //Name
            INodeOption option = GetNodeOptionByName(QUEST_NAME_OPTION);

            if (option.TryGetValue(out string name))
                module.SetQuestName(name);
            //Desc
            option = GetNodeOptionByName(QUEST_DESC_OPTION);

            if (option.TryGetValue(out string desc))
                module.SetQuestDescription(desc);

            //Type
            option = GetNodeOptionByName(QUEST_TYPE_OPTION);

            if (option.TryGetValue(out QuestType type))
                module.SetQuestType(type);
        }
    }
}
