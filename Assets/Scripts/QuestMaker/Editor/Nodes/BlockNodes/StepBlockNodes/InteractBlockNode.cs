using QuestMaker.Data.Steps;
using QuestMaker.Editor.CompilationModules;

using Unity.GraphToolkit.Editor;



namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(ObjectiveNode))]
    [System.Serializable]
    internal class InteractBlockNode : QMBaseStepBlockNode
    {
        public const string ITEM_OPTION = "Item_Option";


        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(ITEM_OPTION, typeof(IInteractable))
                .WithDisplayName("Item")
                .WithDefaultValue(null)
                .Build();
        }

        protected override void ComposeStep(IStepModule module)
        {
            IInteractable item = RetrieveBlockValue<IInteractable>(ITEM_OPTION);


            module.AddStep(new InteractStepData
            {
                Item = item,
            });
        }
    }
}
