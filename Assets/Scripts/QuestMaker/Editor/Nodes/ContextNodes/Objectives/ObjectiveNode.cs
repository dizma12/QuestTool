
using QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.Nodes.SpecialEventNodes;
using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace QuestMaker.Editor.Nodes.ContextNodes
{
    [Serializable]
    internal class ObjectiveNode : QMContextNode
    {
        public const string SPECIAL_EVENT_PORT = "Special_Event_Port";
        public const string OBJECTIVE_FLOW_PORT = "OBJECTIVE_FLOW_PORT";
        public const string OBJECTIVE_DESCRIPTION = "OBJECTIVE_DESCRIPTION";
        public override bool AllowMultipleContextNodesOfSameType => true;
        ObjectiveModule module = null;
        public override Type PortType => typeof(IObjectiveFlowHelper);
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {

            context.AddOutputPort(OBJECTIVE_FLOW_PORT)
                   .WithDataType(typeof(IObjectiveFlowHelper))
                   .WithDisplayName("Next Objective")
                   .WithConnectorUI(PortConnectorUI.Arrowhead)
                   .Build();

            context.AddOutputPort(SPECIAL_EVENT_PORT)
                   .WithDataType(typeof(ISpecialEventNode))
                   .WithDisplayName("Special Event")
                   .WithConnectorUI(PortConnectorUI.Circle)
                   .Build();

            base.OnDefinePorts(context);

        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OBJECTIVE_DESCRIPTION, typeof(string))
                .WithDisplayName("Description")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The description will show on the Quest overlay like {description} {currrentCount}/{MaxCount} (Kill Boars 7/10)")
                .Build();
        }
        public override bool ProcessNode(ModuleBuilderRegistry reg)
        {
            module = reg.RequestNewObjectiveModule<ObjectiveModule>();
            if(module == null) throw new ArgumentNullException("module");

            if (!ProcessBlocks(reg))
                return false;

            CheckForSpecialEvents(reg);
            ProcessSubObjectiveNodes(reg);

            return true;
        }
        protected virtual bool ProcessBlocks(ModuleBuilderRegistry reg)
        {
            IComposableNode[] nodes = GetBlockNodes();
            if (nodes == null || nodes.Length == 0)
                return false;

            

            foreach (var block in nodes)
            {
                block.Compose(module, reg);
            }
            module.SetDescription(RetrieveOptionValue<string>(OBJECTIVE_DESCRIPTION));

            return true;
        }
        protected virtual bool ProcessSubObjectiveNodes(ModuleBuilderRegistry reg)
        {
            IPort output = GetOutputPortByName(OBJECTIVE_FLOW_PORT);
            if (!output.IsConnected)
            {

                return false;
            }

            List<IPort> connected = new();
            output.GetConnectedPorts(connected); ;



            foreach (var portNode in connected)
            {
                if (portNode.GetNode() is not ObjectiveNode node)
                    continue;

                node.ProcessNode(reg);

            }
            return true;
        }
        protected virtual void CheckForSpecialEvents(ModuleBuilderRegistry reg)
        {
            IPort specialEventPort = GetOutputPortByName(SPECIAL_EVENT_PORT);
            if (!specialEventPort.IsConnected) return;

            List<IPort> connected = new();

            specialEventPort.GetConnectedPorts(connected);

            if (connected.Count > 1)
            {
                Debug.LogError("You cant have more than 1 special event to objective nodes");
                return;
            }

            foreach (var port in connected)
            {
                INode node = port.GetNode();
                if (node == null || node is not QMSpecialEventNode specialEventNode) continue;

                specialEventNode.Compose(module, reg);
                Debug.Log("[QMOBjectiveNode Not Implemented]!!!");
            }

        }
    }
}
