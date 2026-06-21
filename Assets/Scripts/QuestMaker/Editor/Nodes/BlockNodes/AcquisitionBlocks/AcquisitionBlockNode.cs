using QuestMaker.Domain;
using QuestMaker.Domain.Quests;
using QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.QuestCompiler.CompilationModules;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(AcquisitionContextNode))]
    [System.Serializable]
    internal class AcquisitionBlockNode : QMBaseBlockNode
    {
        public const string ACQUISITION_OPTION = "ACQUISITION_OPTION";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(ACQUISITION_OPTION, typeof(QuestGiverData))
                .WithDefaultValue(null)
                .WithDisplayName("Hand-in Method")
                .WithTooltip("The method which you acquire the quest")
                .Build();

        }
        public override void Compose(ModuleScope scope)
        {
            QuestGiverData acq = RetrieveBlockValue<QuestGiverData>(ACQUISITION_OPTION);

            if (acq == null)
            {
                ConsoleLogger.LogError(this, "Acquisition method is null");
                return;
            }
            scope.Get<IAcquisitionModule>().SetAcquisitionMethod(acq);
        }
    }
}
