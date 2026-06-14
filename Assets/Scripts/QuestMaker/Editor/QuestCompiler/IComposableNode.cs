
namespace QuestMaker.Editor.CompilationModules
{
    /// <summary>
    /// Implemented by block nodes that write data to a module.
    /// </summary>
    internal interface IComposableNode
    {
        void Compose(ModuleScope scope);
    }
}
