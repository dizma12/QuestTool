using QuestMaker.Data.Steps;
using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Nodes.ContextNodes;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(QMObjectiveContextNode))]
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

        protected override void ComposeStep(ObjectiveModule module)
        {
            string npc = RetrieveBlockValue<string>(NPC_ID);

            module.AddStep(new TalkStepData
            {
                NpcID = npc,
            });
        }
    }
}
