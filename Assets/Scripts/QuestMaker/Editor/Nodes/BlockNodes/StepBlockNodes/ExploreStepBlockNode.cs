using QuestMaker.Data.Steps;
using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Nodes.ContextNodes;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(QMObjectiveContextNode))]
    [System.Serializable]
    internal class ExploreStepBlockNode : QMBaseStepBlockNode
    {
        public const string AREA_OPTION = "Area_Option";


        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(AREA_OPTION, typeof(string))
                .WithDisplayName("Area ID")
                .WithDefaultValue(null)
                .Build();

        }

        protected override void ComposeStep(ObjectiveModule module)
        {
            string area = RetrieveBlockValue<string>(AREA_OPTION);

            module.AddStep(new ExploreStepData
            {
                AreaID = area,
            });
        }
    }
}
