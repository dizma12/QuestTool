using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;
using QuestMaker.Editor.Compiler;
using System;
using System.Linq;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    /// <summary>
    /// Base for all context nodes.
    /// </summary>
    [Serializable]
    internal abstract class QMBaseContextNode : ContextNode
    {
        public abstract bool AllowMultipleContextNodesOfSameType { get; }
        public abstract Type PortType { get; }
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
        /// Returns all block nodes that are IComposableNode.
        /// </summary>
        public virtual IComposableNode[] GetBlockNodes()
        {
            var blocks = BlockNodes.OfType<IComposableNode>().ToArray();

            if (blocks.Length <= 0)
            {
                ConsoleLogger.LogWarning(this, "No IComposableNode blocks found.");
                return null;
            }

            return blocks;
        }

        /// <summary>
        /// Loops all IComposableNodes and passes a module scope.
        /// </summary>
        protected void ComposeBlocks(IQuestModuleBuilder builder, ModuleBuilderRegistry reg)
        {
            IComposableNode[] nodes = GetBlockNodes();
            if (nodes == null) return;

            ModuleScope scope = new(builder, reg);

            foreach (IComposableNode node in nodes)
                node.Compose(scope);
        }

        public abstract bool ProcessNode(ModuleBuilderRegistry reg);

        /// <summary>
        /// Retrieves the value of an option with the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optionName"></param>
        /// <returns></returns>
        protected virtual T RetrieveOptionValue<T>(string optionName)
        {
            INodeOption option = GetNodeOptionByName(optionName);
            option.TryGetValue(out T val);
            return val;
        }
    }
}