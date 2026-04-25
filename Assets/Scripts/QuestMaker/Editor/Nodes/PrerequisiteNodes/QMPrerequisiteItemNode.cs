using QuestMaker.Editor.Compiler;
using QuestMaker.Runtime;
using QuestMaker.Runtime.Data;
using QuestMaker.Runtime.Data.QuestPrerequisites;
using System;

namespace QuestMaker.Editor.Nodes
{
    internal class QMPrerequisiteItemNode : QMBaseOptionNode
    {
        public Item Item = null;

        public override Type PortType => typeof(PrerequisiteData);


        public override void Compose(QuestCompilationContext cntx)
        {
            var preq = new ItemPrerequisiteData
            {
                Item = this.Item
            };
            cntx.AddQuestPrerequisite(preq);
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
