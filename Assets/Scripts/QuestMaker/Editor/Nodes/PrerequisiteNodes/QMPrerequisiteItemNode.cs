using QuestMaker.Data;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;


namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMPrerequisiteItemNode : QMBaseOptionNode
    {
        public Item Item = null;

        public override Type PortType => typeof(IOptionNode);


        public override void Compose<T>(T bldr, ModuleBuilderRegistry cntx)
        {
            Item = RetrieveNodeOption<Item>();
            IItemModule module = cntx.RequestModule<T, IItemModule>();
            module?.SetItem(Item);

        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OPTION_NODE_PORT, typeof(Item))
                .WithDefaultValue(default)
                .WithDisplayName("Item")
                .Build();
        }
    }
}
