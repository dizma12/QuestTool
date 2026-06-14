using QuestMaker.Domain.Steps;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
    internal class TalkStepBlockNode : QMBaseStepBlockNode
    {
        public const string NPC_ID = "NPC_Option";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(NPC_ID, typeof(string))
                .WithDisplayName("NPC ID")
                .WithDefaultValue(null)
                .Build();
        }

        protected override void ComposeStep(IStepModule module)
        {
            string npc = RetrieveBlockValue<string>(NPC_ID);

            module.AddStep(new TalkStepData
            {
                NpcID = npc,
            });
        }
    }
}
