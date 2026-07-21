using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;

using Unity.GraphToolkit.Editor;


namespace QuestMaker.Editor.Nodes.BlockNodes
{

    [UseWithContext(typeof(RewardNode))]
    [System.Serializable]
    internal class AbilityBlockNode : QMBaseBlockNode
    {
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(string))
                .WithDefaultValue(string.Empty)
                .WithDisplayName("Ability ID")
                .WithTooltip("The id of the ability")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            string ability = RetrieveBlockValue<string>();
            if (string.IsNullOrEmpty(ability))
            {
                ConsoleLogger.LogError(this, "Ability string is null");
                return;
            }
            scope?.Get<IAbilityModule>().SetAbility(ability);
        }
    }
}
