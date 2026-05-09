using QuestMaker.Editor.Compiler;
using System;
using System.Diagnostics;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal abstract class QMBaseContextNode : ContextNode
    {
        /// <summary>
        /// Input port for Graph Flow
        /// </summary>
        public const string INPUT_PORT = "FlowIn";

        /// <summary>
        /// Output port for Graph Flow
        /// </summary>
        public const string OUTPUT_PORT = "FlowOut";

        public abstract Type PortType { get; }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            
            context.AddInputPort(INPUT_PORT)
                .WithDataType(typeof(QMFlowHelper))
                .WithDefaultValue(default)
                .WithDisplayName(INPUT_PORT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort(OUTPUT_PORT)
                .WithDataType(typeof(QMFlowHelper))
                .WithDisplayName(OUTPUT_PORT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }


        public virtual bool ProcessBlockNodes()
        {
            var blocks = BlockNodes.OfType<IComposableNode>().ToArray();
            if (blocks.Length <= 0)
            {
                UnityEngine.Debug.LogWarning($"Failed to find any valid blocks of type IComposableNode");
                return false;
            }


            return true;
        }
    }
}
