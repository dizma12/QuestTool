using System;
using QuestMaker.Data.StepsAndObjectives;
using QuestMaker.Editor.Compiler.CompilationModules;


namespace QuestMaker.Editor.Nodes.Steps
{
    [Serializable]
    internal class QMStepNode : QMBaseOptionNode
    {
        public override Type PortType => typeof(IOptionNode);

        public QuestStepSO stepType = null;

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OPTION_NODE_PORT, typeof(QuestStepSO))
                   .WithDefaultValue(null)
                   .WithDisplayName("Quest Type")
                   .Build();

        }
        public override void Compose<T>(T bldr, ModuleBuilderRegistry cntx)
        {
            throw new NotImplementedException();
        }
    }

}
