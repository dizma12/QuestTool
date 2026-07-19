using QuestMaker.Domain;
using QuestMaker.Domain.Steps;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
    internal class DeliverStepBlockNode : QMBaseStepBlockNode
    {

        public const string ITEM_OPTION = "ITEM_OPTION";
        public const string NPC_OPTION = "NPC_OPTION";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(ITEM_OPTION, typeof(Item))
                .WithDisplayName("Item")
                .WithDefaultValue(null)
                .Build();

            context.AddOption(NPC_OPTION, typeof(string))
                .WithDisplayName("NPC ID")
                .WithDefaultValue(string.Empty)
                .Build();
        }
        protected override void ComposeStep(IStepModule module)
        {
            Item item = RetrieveBlockValue<Item>(ITEM_OPTION);
            string npc = RetrieveBlockValue<string>(NPC_OPTION);

            module.AddStep(new DeliverStepData
            {
                Item = item,
                NpcID = npc,
            });
        }
    }
}
