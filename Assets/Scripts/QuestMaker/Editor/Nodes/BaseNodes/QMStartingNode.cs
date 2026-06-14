using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;
using System;
using System.Collections.Generic;
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
            context.AddOption(QUEST_TYPE_OPTION, typeof(QuestType))
                .WithDisplayName("Quest Type")
                .WithDefaultValue(QuestType.Main)
                .WithTooltip("The type of the quest that is used to categorize it at runtime.")
                .Build();

            context.AddOption(QUEST_NAME_OPTION, typeof(string))
                .WithDisplayName("Name")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The name of the quest.")
                .Build();

            context.AddOption(QUEST_DESC_OPTION, typeof(string))
                .WithDisplayName("Description")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The description of the quest.")
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
                .WithDisplayName("Objectives")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        public virtual void ProcessNode(ModuleBuilderRegistry reg)
        {

            QuestInfoModule builder = reg.GetBuilder<QuestInfoModule>();
            if (builder == null) return;

            IQuestInfoModule module = builder as IQuestInfoModule;


            if (GetNodeOptionByName(QUEST_TYPE_OPTION).TryGetValue(out QuestType type))
                module.SetQuestType(type);

            if (GetNodeOptionByName(QUEST_NAME_OPTION).TryGetValue(out string name))
                module.SetQuestName(name);

            if (GetNodeOptionByName(QUEST_DESC_OPTION).TryGetValue(out string desc))
                module.SetQuestDescription(desc);


            //Special Events
            IPort specialEventPort = GetOutputPortByName(SPECIAL_EVENT_PORT);
            if (!specialEventPort.IsConnected) return;

            List<IPort> connected = new();
            specialEventPort.GetConnectedPorts(connected);
            if (connected.Count == 0) return;

            //Module Scope falls off here but its onnly for this node.
            ModuleScope scope = new(builder, reg);

            foreach (IPort port in connected)
            {
                INode portNode = port.GetNode();
                if (portNode == null || portNode is not QMSpecialEventNode specialEventNode) continue;

                specialEventNode.Compose(scope);
            }
        }
    }
}