using System;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal abstract class QMBaseHubNode : QMBaseNode
    {
        public const string HUB_OUTPUT_PORT = "Hub_Output";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(HUB_OUTPUT_PORT)
                .WithDataType(PortType)
                .WithDisplayName("Options")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            base.OnDefinePorts(context);

        }
    }
}
