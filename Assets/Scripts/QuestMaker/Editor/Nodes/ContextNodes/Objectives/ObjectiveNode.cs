using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;
using QuestMaker.Editor.Compiler;
using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;


namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class ObjectiveNode : QMBaseContextNode
    {
        public const string OBJECTIVE_FLOW_PORT = "OBJECTIVE_FLOW_PORT";
        public const string OBJECTIVE_DESCRIPTION = "OBJECTIVE_DESCRIPTION";

        public override bool AllowMultipleContextNodesOfSameType => true;
        public override Type PortType => typeof(IObjectiveFlowHelper);

        private ObjectiveModule module = null;

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(OBJECTIVE_FLOW_PORT)
                   .WithDataType(typeof(IObjectiveFlowHelper))
                   .WithDisplayName("Next Objective")
                   .WithConnectorUI(PortConnectorUI.Arrowhead)
                   .Build();

            base.OnDefinePorts(context);
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OBJECTIVE_DESCRIPTION, typeof(string))
                .WithDisplayName("Description")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The description will show on the Quest overlay like {description} {currentCount}/{MaxCount} (Kill Boars 7/10)")
                .Build();
        }

        public override bool ProcessNode(ModuleBuilderRegistry reg)
        {
            module = reg.RequestNewObjectiveModule<ObjectiveModule>() ?? throw new ArgumentNullException(nameof(module));

            if (!ProcessBlocks(reg))
                return false;

            ProcessSubObjectiveNodes(reg);

            return true;
        }

        protected virtual bool ProcessBlocks(ModuleBuilderRegistry reg)
        {
            IComposableNode[] nodes = GetBlockNodes();
            if (nodes == null || nodes.Length == 0)
                return false;

            ModuleScope scope = new(module, reg);

            foreach (var block in nodes)
                block.Compose(scope);

            module.SetDescription(RetrieveOptionValue<string>(OBJECTIVE_DESCRIPTION));
            return true;
        }

        protected virtual bool ProcessSubObjectiveNodes(ModuleBuilderRegistry reg)
        {
            IPort output = GetOutputPortByName(OBJECTIVE_FLOW_PORT);
            if (!output.IsConnected) return false;

            List<IPort> connected = new();
            output.GetConnectedPorts(connected);

            foreach (var port in connected)
            {
                if (port.GetNode() is not ObjectiveNode node) continue;
                node.ProcessNode(reg);
            }

            return true;
        }
    }
}