using QuestMaker.Editor.Nodes;
using QuestMaker.Editor.Compiler;
using System;
using Unity.GraphToolkit.Editor;
using QuestMaker.Runtime.Data;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal abstract class QMBaseOptionNode : QMBaseNode , IComposableNode
    {
        public const string OPTION_NODE_FLOW_INPUT_PORT = "Option_Flow_Input";
        public const string OPTION_NODE_PORT = "Option_Input";  
        
        public virtual string OPTION_NODE_INPUT_DISPLAY_NAME { get => QMBaseNode.INPUT_PORT; }

        public abstract void Compose(QuestCompilationContext cntx);

        
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
