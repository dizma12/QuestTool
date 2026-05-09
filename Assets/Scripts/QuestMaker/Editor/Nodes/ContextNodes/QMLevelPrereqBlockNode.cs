using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(QMPrerequisiteContextNode))]
    [Serializable]
    internal class QMLevelPrereqBlockNode : QMBaseBlockNode, IComposableNode
    {
        int Level;

        public override void Compose(ModuleRegistry cntx)
        {
            throw new NotImplementedException();
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(int))
                .WithDefaultValue(0)
                .WithDisplayName("Level")
                .Build();
        }
    }
}
