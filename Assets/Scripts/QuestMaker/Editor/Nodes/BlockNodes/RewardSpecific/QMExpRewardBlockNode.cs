using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Nodes;
using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;


namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(QMRewardContextNode))]
    [Serializable]
    internal class QMExpRewardBlockNode : QMBaseBlockNode
    {
        private int _exp = 0;
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(int))
                .WithDefaultValue(0)
                .WithDisplayName("Exp amount")
                .WithTooltip("The exp amount to add")
                .Build();
        }
        public override void Compose<TBuilder>(TBuilder bldr, ModuleBuilderRegistry reg)
        {
            _exp = RetrieveBlockValue<int>();
            IExpModule module = reg.GetModule<TBuilder, IExpModule>();

            if (module != null )
            {
                module.SetExp(_exp);
            }
            else Debug.Log($"Failed set the Exp on module");
        }
    }
}
