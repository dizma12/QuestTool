using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal abstract class QMBaseBlockNode : BlockNode , IComposableNode
    {
        public const string BLOCK_NODE_OPTION = "Block_Option";

        public abstract void Compose(ModuleRegistry cntx);


        protected virtual T RetrieveBlockValue<T>(string optionName = BLOCK_NODE_OPTION)
        {
            INodeOption option = GetNodeOptionByName(optionName);

            option.TryGetValue(out T val);
            return val;
        }
    }
}
