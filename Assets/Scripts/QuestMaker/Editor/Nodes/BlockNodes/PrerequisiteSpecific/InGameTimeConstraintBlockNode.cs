using QuestMaker.Data;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;


namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(PrerequisiteNode))]
    [System.Serializable]
    internal class InGameTimeConstraintBlockNode : QMBaseBlockNode, IComposableNode
    {

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(InGameTimeline))
                .WithDefaultValue(InGameTimeline.None)
                .WithDisplayName("Time Constraint")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            InGameTimeline time = RetrieveBlockValue<InGameTimeline>();
            scope.Get<IInGameTimeConstraintModule>()?.SetTimeConstraint(time);
        }
    }
}
