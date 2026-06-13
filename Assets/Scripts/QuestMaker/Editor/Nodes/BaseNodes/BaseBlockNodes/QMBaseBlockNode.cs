using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{

    [System.Serializable]
    internal abstract class QMBaseBlockNode : BlockNode, IComposableNode
    {
        public const string BLOCK_NODE_OPTION = "Block_Option";

        protected virtual T RetrieveBlockValue<T>(string optionName = BLOCK_NODE_OPTION)
        {
            INodeOption option = GetNodeOptionByName(optionName);

            option.TryGetValue(out T val);
            return val;
        }

        //public abstract void Compose<TBuilder>(TBuilder bldr, ModuleBuilderRegistry reg) where TBuilder : class, IQuestModuleBuilder, new();
        public abstract void Compose(ModuleScope scope);

    }
}
