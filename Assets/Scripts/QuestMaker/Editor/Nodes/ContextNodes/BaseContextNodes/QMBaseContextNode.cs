using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using System.Linq;
using Unity.GraphToolkit.Editor;
namespace QuestMaker.Editor.Nodes
{
    /// <summary>
    /// Class that all context nodes derive from (closed).
    /// </summary>
    [Serializable]
    internal abstract class QMContextNode : ContextNode
    {
        public abstract bool AllowMultipleContextNodesOfSameType { get; }
        public abstract Type PortType { get; }

        /// <summary>
        /// Input port for Graph Flow
        /// </summary>
        public const string INPUT_PORT = "FlowIn";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(INPUT_PORT)
                .WithDataType(PortType)
                .WithDefaultValue(default)
                .WithDisplayName(INPUT_PORT)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        /// <summary>
        /// Locates all blocks of the context Node.
        /// </summary>
        /// <param name="cntx"></param>
        /// <returns>IComposable Array or Null if doesnt find any.</returns>
        public virtual IComposableNode[] GetBlockNodes()
        {
            var blocks = BlockNodes.OfType<IComposableNode>().ToArray();
            if (blocks.Length <= 0)
            {
                UnityEngine.Debug.LogWarning($"Failed to find any valid blocks of type IComposableNode");

                return null;
            }

            return blocks;
        }

        public abstract bool ProcessNode(ModuleBuilderRegistry reg);


        protected virtual T RetrieveOptionValue<T>(string optionName)
        {
            INodeOption option = GetNodeOptionByName(optionName);

            option.TryGetValue(out T val);
            return val;
        }
    }


    /// <summary>
    /// Base Class that all non-objective context nodes should derive from.
    /// </summary>
    [Serializable]
    internal abstract class QMBaseContextNode : QMContextNode 
    { 
        public override Type PortType => typeof(IContextFlowHelper); 
    };
}

