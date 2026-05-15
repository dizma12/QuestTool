using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Nodes;
using QuestMaker.Runtime.Data;
using System;
using Unity.GraphToolkit.Editor;
using UnityEditor.ShaderGraph.Internal;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal abstract class QMBaseOptionNode : QMBaseNode, IComposableNode
    {
        public const string OPTION_NODE_FLOW_INPUT_PORT = "Option_Flow_Input";
        public const string OPTION_NODE_PORT = "Option_Input";

        public virtual string OPTION_NODE_INPUT_DISPLAY_NAME { get => QMBaseOptionNode.INPUT_PORT; }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(OPTION_NODE_FLOW_INPUT_PORT)
                .WithDataType(PortType)
                .WithDisplayName(OPTION_NODE_INPUT_DISPLAY_NAME)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .WithDefaultValue(default)
                .Build();
        }

        /// <summary>
        /// Reads the Node option of type T. ***DOES NOT CHECK NULL***
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionName"></param>
        /// <returns></returns>
        protected T RetrieveNodeOption<T>(string optionName = OPTION_NODE_PORT)
        {
            INodeOption option = GetNodeOptionByName(optionName);

            option.TryGetValue(out T val);
            return val;
        }

        public abstract void Compose<T>(T bldr, ModuleBuilderRegistry cntx) where T: class, IQuestModuleBuilder, new();

    }
}
