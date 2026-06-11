using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Nodes.ContextNodes;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
    internal abstract class QMBaseStepBlockNode : QMBaseBlockNode
    {
        public override void Compose<TBuilder>(TBuilder bldr, ModuleBuilderRegistry reg)
        {
            // Concrete blocks compose themselves into an ObjectiveModule

            if (bldr is not ObjectiveModule objectiveModule) return;
            ComposeStep(objectiveModule);
        
        }
        
        // Since all StepBlocks use Objective Module we dont have to repeat the cast in every subclass
        protected abstract void ComposeStep(ObjectiveModule module);
    }
}
