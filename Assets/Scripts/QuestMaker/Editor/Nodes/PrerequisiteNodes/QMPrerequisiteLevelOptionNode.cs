using QuestMaker.Editor.Compiler;
using QuestMaker.Runtime.Data;
using QuestMaker.Runtime.Data.QuestPrerequisites;
using System;

namespace QuestMaker.Editor.Nodes
{
    internal class QMPrerequisiteLevelOptionNode : QMBaseOptionNode<PrerequisiteData>
    {
        public int Level = 0;

        public override Type PortType { get => typeof(PrerequisiteData); }

        public override PrerequisiteData Compose(QuestCompilationContext cntx)
        {
            var preq = new LevelPrerequisiteData
            {
                Level = this.Level
            };
            cntx.AddQuestPrerequisite(preq);
            return preq;
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OPTION_NODE_PORT, typeof(int))
                .WithDefaultValue(default)
                .WithDisplayName("Level")
                .Build();
        }

    }
}
