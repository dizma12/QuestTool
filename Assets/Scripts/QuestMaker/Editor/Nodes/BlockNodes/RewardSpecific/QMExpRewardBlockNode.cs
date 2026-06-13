using QuestMaker.Editor.CompilationModules;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace QuestMaker.Editor.Nodes.BlockNodes
{
    [UseWithContext(typeof(RewardNode))]
    [System.Serializable]
    internal class QMExpRewardBlockNode : QMBaseBlockNode
    {
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(int))
                .WithDefaultValue(0)
                .WithDisplayName("Exp amount")
                .WithTooltip("The exp amount to add")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            int exp = RetrieveBlockValue<int>();
            IExpModule module = scope.Get<IExpModule>();

            if (module != null)
                module.SetExp(exp);
            else
                Debug.LogWarning("[QMExpRewardBlockNode] IExpModule not found on scope.");
        }
    }
}
