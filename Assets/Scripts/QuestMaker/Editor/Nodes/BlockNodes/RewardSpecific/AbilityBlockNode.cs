using QuestMaker.Editor.Compiler.CompilationModules;
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
                .Build();
        }

        public override void Compose<TBuilder>(TBuilder bldr, ModuleBuilderRegistry reg)
        {
            string option = RetrieveBlockValue<string>(BLOCK_NODE_OPTION);

            if (string.IsNullOrEmpty(option)) return;
                
            reg.GetModuleByBuilder<TBuilder, IAbilityModule>(bldr).SetAbility(option);
        }
    }
}
