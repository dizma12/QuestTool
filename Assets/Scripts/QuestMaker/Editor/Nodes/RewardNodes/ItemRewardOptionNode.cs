using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime.Data;
using System;


namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class ItemRewardOptionNode : QMBaseOptionNode
    {
        public override Type PortType => typeof(IOptionNode);

        public override void Compose(QuestModuleBuilder cntx)
        {
            throw new NotImplementedException();
        }
    }

    internal interface IOptionNode { }
    internal interface IRewardOptionNode : IOptionNode { }
    internal interface IPrerequisiteOptionNode : IOptionNode { }
    internal interface IReputationOptionNode : IOptionNode { }
}
