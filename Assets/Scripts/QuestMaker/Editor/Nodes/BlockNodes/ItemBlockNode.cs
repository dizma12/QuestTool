using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(QMBaseContextNode))]
    [System.Serializable]
    internal class ItemBlockNode : QMBaseBlockNode
    {
        public const string AMOUNT_OPTION = "AMOUNT_OPTION";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(Item))
                .WithDefaultValue(null)
                .WithDisplayName("Item")
                .Build();

            context.AddOption(AMOUNT_OPTION, typeof(int))
                .WithDefaultValue(1)
                .WithDisplayName("Amount")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            Item item = RetrieveBlockValue<Item>();
            int amount = RetrieveBlockValue<int>(AMOUNT_OPTION);

            if (item == null)
            {
                ConsoleLogger.LogWarning(this, "Item is null.");
                return;
            }
            if (amount <= 0)
            {
                ConsoleLogger.LogWarning(this, "Amount is <= 0.");
                return;
            }

            scope.Get<IItemModule>()?.SetItem(item, amount);
        }
    }
}