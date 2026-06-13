
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(QMBaseContextNode))]
    [System.Serializable]
    internal class ReputationBlockNode : QMBaseBlockNode
    {
        public const string FACTION_OPTION = "FACTION_OPTION";
        public const string AMOUNT_OPTION = "AMOUNT_OPTION";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(FACTION_OPTION, typeof(string))
                .WithDefaultValue(null)
                .WithDisplayName("Faction ID")
                .Build();

            context.AddOption(AMOUNT_OPTION, typeof(int))
                .WithDefaultValue(1)
                .WithDisplayName("Amount")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            string faction = RetrieveBlockValue<string>(FACTION_OPTION);
            int amount = RetrieveBlockValue<int>(AMOUNT_OPTION);

            scope.Get<IReputationModule>()?.SetReputationFaction(new() { FactionID = faction, Amount = amount });
        }
    }
}

