using QuestMaker.Domain;
using QuestMaker.Domain.Quests;
using QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.QuestCompiler.CompilationModules;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(AcquisitionContextNode))]
    [System.Serializable]
    internal class TurnInBlockNode : QMBaseBlockNode
    {
        public const string TURN_IN_OPTION = "TURN_IN_OPTION";
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {

            context.AddOption(TURN_IN_OPTION, typeof(QuestGiverData))
                .WithDefaultValue(null)
                .WithDisplayName("Turn-in Method")
                .WithTooltip("The method which you acquire the quest")
                .Build();
        }
        public override void Compose(ModuleScope scope)
        {
            QuestGiverData turnin = RetrieveBlockValue<QuestGiverData>(TURN_IN_OPTION);
            if(turnin == null)
            {
                ConsoleLogger.LogError(this, "Turn-In method is null");
                return;
            }
            scope.Get<IAcquisitionModule>().SetTurnInMethod(turnin);
        }
    }
}
