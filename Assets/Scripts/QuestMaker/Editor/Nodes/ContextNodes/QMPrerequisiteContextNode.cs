using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMPrerequisiteContextNode : QMBaseContextNode
    {
        
        public override Type PortType => typeof(IOptionNode);
    }
}
