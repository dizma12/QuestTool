using QuestMaker.Editor.Nodes;
using System;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    internal abstract class QMBaseOptionNode : QMBaseNode
    {
        public const string OPTION_NODE_FLOW_INPUT_PORT = "Option_Flow_Input";
        public const string OPTION_NODE_PORT = "Option_Input";  
        
        public virtual string OPTION_NODE_INPUT_DISPLAY_NAME { get => QMBaseNode.OUTPUT_PORT; }
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(OPTION_NODE_FLOW_INPUT_PORT)
                .WithDataType(PortType)
                .WithDisplayName(OPTION_NODE_INPUT_DISPLAY_NAME)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .WithDefaultValue(default)
                .Build();
        }
    }
}
