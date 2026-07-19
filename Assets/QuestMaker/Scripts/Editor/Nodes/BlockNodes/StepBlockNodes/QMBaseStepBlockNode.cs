using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;
 
namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
    internal abstract class QMBaseStepBlockNode : QMBaseBlockNode
    {
        public override void Compose(ModuleScope scope)
        {
            IStepModule module = scope.Get<IStepModule>();
            if (module == null) return;

            ComposeStep(module);
        }

        /// <summary>
        /// Subclasses build and add their specific QuestStepData through IStepModule.
        /// No concrete ObjectiveModule reference needed.
        /// </summary>
        protected abstract void ComposeStep(IStepModule module);
    }
}