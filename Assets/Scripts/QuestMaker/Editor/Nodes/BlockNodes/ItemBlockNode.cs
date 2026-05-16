using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Data;
using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(QMBaseContextNode))]
    [Serializable]
    internal class ItemBlockNode : QMBaseBlockNode
    {
        Item item = null;
        int amount = 1;
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
        public override void Compose<T>(T bldr, ModuleBuilderRegistry cntx)
        {
            item = RetrieveBlockValue<Item>();
            amount = RetrieveBlockValue<int>(AMOUNT_OPTION);

            IItemModule module = cntx.GetModule<T, IItemModule>();

            module?.SetItem(item, amount);
        }
    }
}
