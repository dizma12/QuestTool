using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Editor.Compiler
{
    internal interface IComposableNode
    {
        void Compose(QuestCompilationContext cntx);
    }
}
