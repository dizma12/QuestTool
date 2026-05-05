using QuestMaker.Editor.Compiler;
using QuestMaker.Runtime.Data;
using QuestMaker.Runtime.Data.QuestPrerequisites;
using System;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMPrerequisiteLevelOptionNode : QMBaseOptionNode
    {
        public int Level = 0;

        public override Type PortType { get => typeof(PrerequisiteData); }

        public override void Compose(QuestCompilationContext cntx)
        {
            var preq = new LevelPrerequisiteData
            {
                Level = this.Level
            };
            cntx.AddQuestPrerequisite(preq);

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
