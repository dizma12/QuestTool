using QuestMaker.Editor.Compiler;
using QuestMaker.Runtime;
using QuestMaker.Runtime.Data;
using QuestMaker.Runtime.Data.QuestPrerequisites;
using System;

namespace QuestMaker.Editor.Nodes
{
    internal class QMPrerequisiteItemNode : QMBaseOptionNode<PrerequisiteData>
    {
        public Item Item = null;

        public override Type PortType => typeof(PrerequisiteData);


        public override PrerequisiteData Compose(QuestCompilationContext cntx)
        {
            return new ItemPrerequisiteData
            {
                Item = this.Item
            };
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
