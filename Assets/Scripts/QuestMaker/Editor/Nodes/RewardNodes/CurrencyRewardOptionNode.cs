using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime.Data;
using System;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class CurrencyRewardOptionNode : QMBaseOptionNode, IRewardOptionNode
    {
        public override Type PortType => typeof(IRewardOptionNode);

        public override void Compose(QuestModuleBuilder cntx)   
        {
            throw new NotImplementedException();
        }
    }
}
