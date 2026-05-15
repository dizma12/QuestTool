using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime;
using QuestMaker.Runtime.Data;
using System;
using UnityEngine;

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
            IItemModule module = cntx.GetModule<T, IItemModule>();
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
