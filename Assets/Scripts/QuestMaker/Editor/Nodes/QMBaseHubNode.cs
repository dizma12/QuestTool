using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
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
