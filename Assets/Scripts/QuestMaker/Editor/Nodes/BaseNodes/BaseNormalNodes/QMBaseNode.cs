using System;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal abstract class QMBaseNode : Node
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
                .WithDataType(PortType)
                .WithDefaultValue(default)
                .WithDisplayName(INPUT_PORT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

        }
    }

}
