using QuestMaker.Domain;
using QuestMaker.Domain.Steps;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
    internal class CollectStepBlockNode : QMBaseStepBlockNode
    {
        public const string ITEM_OPTION = "ITEM_OPTION";
        public const string AMOUNT_OPTION = "AMOUNT_OPTION";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(ITEM_OPTION, typeof(Item))
                .WithDisplayName("Item")
                .WithDefaultValue(null)
                .Build();

            context.AddOption(AMOUNT_OPTION, typeof(int))
                .WithDisplayName("Amount")
                .WithDefaultValue(1)
                .Build();
        }

        protected override void ComposeStep(IStepModule module)
        {
            Item item = RetrieveBlockValue<Item>(ITEM_OPTION);
            int amount = RetrieveBlockValue<int>(AMOUNT_OPTION);

            module.AddStep(new CollectStepData
            {
                CollectableItem = new() { Item = item , Amount = amount}
            });
        }
    }
}
