using QuestMaker.Data.Steps;
using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
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

        protected override void ComposeStep(IStepModule module)
        {
            string area = RetrieveBlockValue<string>(AREA_OPTION);

            module.AddStep(new ExploreStepData
            {
                AreaID = area,
            });
        }
    }
}
