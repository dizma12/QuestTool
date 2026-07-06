using QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.QuestCompiler.CompilationModules;
using QuestMaker.Editor.CompilationModules;
using System;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class AcquisitionContextNode : QMBaseContextNode
    {
        public override bool AllowMultipleContextNodesOfSameType => false;

        public override Type PortType => typeof(IContextFlowHelper);

        public override bool ProcessNode(ModuleBuilderRegistry reg)
        {
            AcquisitionModule builder = reg.GetBuilder<AcquisitionModule>();
            if (builder == null) return false;

            ComposeBlocks(builder, reg);
            return true;
        }
    }
}
