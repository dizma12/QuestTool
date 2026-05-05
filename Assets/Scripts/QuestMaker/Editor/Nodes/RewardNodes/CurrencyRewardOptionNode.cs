using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class CurrencyRewardOptionNode : QMBaseOptionNode
    {
        public override Type PortType => typeof(RewardData);

        public override void Compose(QuestModuleContext cntx)
        {
            throw new NotImplementedException();
        }
    }
}
