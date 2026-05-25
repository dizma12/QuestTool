using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
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

        public abstract bool AllowMultipleContextNodesOfSameType { get; }
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

        /// <summary>
        /// Locates all block nodes of the context block.
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

        public abstract bool ProccessNodes(ModuleBuilderRegistry reg);
        
    }
}
