using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class RewardNode : QMBaseContextNode
    {
        public override bool AllowMultipleContextNodesOfSameType { get => false; }

        public override bool ProcessNode(ModuleBuilderRegistry reg)
        {
            RewardModule builder = reg.GetBuilder<RewardModule>(); 

            if(builder == null) return false;
            IComposableNode[] nodes = GetBlockNodes();

            foreach(IComposableNode node in nodes)
            {
                node.Compose(builder, reg);
            }
            return true;
        }

        
    }
}
