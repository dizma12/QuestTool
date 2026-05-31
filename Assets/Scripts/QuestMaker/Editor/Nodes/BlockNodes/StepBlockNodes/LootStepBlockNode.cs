using QuestMaker.Data;
using QuestMaker.Data.Steps;
using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Nodes.ContextNodes;

using Unity.GraphToolkit.Editor;


namespace QuestMaker.Editor.Nodes.BlockNodes
{ 
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
internal class LootStepBlockNode : QMBaseStepBlockNode
    {
        public const string ITEM_OPTION = "Item_Option";
        public const string AMOUNT_OPTION = "Amount_Option";

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

        protected override void ComposeStep(ObjectiveModule module)
        {
            Item item = RetrieveBlockValue<Item>(ITEM_OPTION);
            int amount = RetrieveBlockValue<int>(AMOUNT_OPTION);

            module.AddStep(new LootStepData
            {
                Item = item,
                Amount = amount
            });
        }
    }
}