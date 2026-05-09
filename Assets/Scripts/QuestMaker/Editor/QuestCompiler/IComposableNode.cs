
using QuestMaker.Editor.Compiler.CompilationModules;

namespace QuestMaker.Editor.Compiler
{
    internal interface IComposableNode
    {
        void Compose(ModuleRegistry cntx);
    }
}
