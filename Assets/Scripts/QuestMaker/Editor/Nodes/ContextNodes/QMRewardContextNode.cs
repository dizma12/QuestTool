using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMRewardContextNode : QMBaseContextNode
    {
        public override Type PortType => typeof(IOptionNode);

        public override bool ProccessNodes(ModuleBuilderRegistry reg)
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
