using QuestMaker.Domain;
using QuestMaker.Domain.Steps;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;


namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
    internal class SlayStepBlockNode : QMBaseStepBlockNode
    {
        public const string ENEMY_ID_OPTION = "EnemyID_Option";
        public const string AMOUNT_OPTION = "Amount_Option";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(ENEMY_ID_OPTION, typeof(string))
                .WithDisplayName("Enemy ID")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The ID of the enemy type to slay")
                .Build();

            context.AddOption(AMOUNT_OPTION, typeof(int))
                .WithDisplayName("Amount")
                .WithDefaultValue(1)
                .WithTooltip("How many must be slain")
                .Build();
        }

        protected override void ComposeStep(IStepModule module)
        {
            string enemyID = RetrieveBlockValue<string>(ENEMY_ID_OPTION);
            int amount = RetrieveBlockValue<int>(AMOUNT_OPTION);

            SlayStepData step = new SlayStepData
            {
                EnemyID = enemyID,
                Amount = amount
            };

            module.AddStep(step);

            ConsoleLogger.Log(this, $"Added step with id= {step.StepID}");
        }
    }
}