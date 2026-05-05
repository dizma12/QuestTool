using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime.Data;
using System;


namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class ItemRewardOptionNode : QMBaseOptionNode
    {
        public override Type PortType => typeof(RewardData);

        public override void Compose(QuestModuleContext cntx)
        {
            throw new NotImplementedException();
        }
    }
}
