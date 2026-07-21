using QuestMaker.Editor.CompilationModules;
using QuestMaker.Editor.Compiler;

using Unity.GraphToolkit.Editor;
using UnityEngine;
using static UnityEditor.ObjectChangeEventStream;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(PrerequisiteNode))]
    [System.Serializable]
    internal class LevelBlockNode : QMBaseBlockNode, IComposableNode
    {
        int Level;


        public override void Compose(ModuleScope scope)
        {
            Level = RetrieveBlockValue<int>();
            scope.Get<ILevelModule>()?.SetLevel(Level);
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
