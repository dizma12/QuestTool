using QuestMaker.Editor.CompilationModules;
using QuestMaker.Editor.Compiler;
using System;


namespace QuestMaker.Editor.Nodes
{
    [System.Serializable]
    internal class PrerequisiteNode : QMBaseContextNode
    {
        public override bool AllowMultipleContextNodesOfSameType => false;

        public override Type PortType => typeof(IContextFlowHelper);

        public override bool ProcessNode(ModuleBuilderRegistry reg)
        {
            PrerequisiteModule builder = reg.GetBuilder<PrerequisiteModule>();
            if (builder == null) return false;

            ComposeBlocks(builder, reg);
            return true;
        }
    }
}
