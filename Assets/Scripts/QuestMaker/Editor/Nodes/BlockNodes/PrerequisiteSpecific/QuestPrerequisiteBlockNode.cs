using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(PrerequisiteNode))]
    [System.Serializable]
    internal class QuestPrerequisiteBlockNode : QMBaseBlockNode, IComposableNode
    {
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(QuestSO))
                .WithDefaultValue(null)
                .WithDisplayName("Required Quest")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            QuestSO quest = RetrieveBlockValue<QuestSO>();

            if (quest == null)
            {
                ConsoleLogger.LogWarning(this, "Quest is null.");
                return;
            }

            scope.Get<IQuestPrerequisiteModule>()?.SetQuestPrerequisite(quest);
        }
    }
}
