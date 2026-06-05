using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(QMBaseContextNode))]
    [Serializable]
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
        public override void Compose<TBuilder>(TBuilder bldr, ModuleBuilderRegistry reg)
        {
            string faction = RetrieveBlockValue<string>(FACTION_OPTION);
            int amount = RetrieveBlockValue<int>(AMOUNT_OPTION);

            IReputationModule module = reg.RequestModule<TBuilder, IReputationModule>();

            module.SetReputationFaction(new() { FactionID = faction, Amount = amount });
        }
    }
}
