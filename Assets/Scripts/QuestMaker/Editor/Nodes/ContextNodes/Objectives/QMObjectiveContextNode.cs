using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;

namespace QuestMaker.Editor.Nodes.ContextNodes
{
    [Serializable]
    internal class QMObjectiveContextNode : QMBaseContextNode
    {
        public override bool AllowMultipleContextNodesOfSameType => true;

        public override Type PortType => typeof(IOptionNode);

        public override bool ProccessNodes(ModuleBuilderRegistry reg)
        {

            IComposableNode[] nodes = GetBlockNodes();
            if(nodes == null || nodes.Length == 0)
                return false;

            ObjectiveModule module = reg.RequestNewObjectiveModule<ObjectiveModule>();

            foreach (var block in nodes)
            {
                block.Compose(module, reg);
            }

            return true;
        }
    }
}
