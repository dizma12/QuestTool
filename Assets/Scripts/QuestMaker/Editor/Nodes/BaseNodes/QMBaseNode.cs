using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Configuration;
using System.Text;
using System.Threading.Tasks;
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
    }


    /// <summary>
    /// This is an empty class used to controll the flow of the Graph.
    /// HOLDS NO DATA OR LOGIC.
    /// </summary>
    internal class QMFlowHelper { }

}
