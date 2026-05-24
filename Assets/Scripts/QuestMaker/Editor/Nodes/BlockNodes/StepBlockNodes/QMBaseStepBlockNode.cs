using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [Serializable]
    internal abstract class QMBaseStepBlockNode : QMBaseBlockNode
    {
        public override void Compose<TBuilder>(TBuilder bldr, ModuleBuilderRegistry reg)
        {
            // Concrete blocks compose themselves into an ObjectiveModule

            if (bldr is not ObjectiveModule objectiveModule) return;
            ComposeStep(objectiveModule);
        
        }
        // Each concrete block implements this instead — cleaner than
        // repeating the cast in every subclass
        protected abstract void ComposeStep(ObjectiveModule module);
    }
}
