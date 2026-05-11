using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(QMPrerequisiteContextNode))]
    [Serializable]
    internal class QMLevelPrereqBlockNode : QMBaseBlockNode, IComposableNode
    {
        int Level;

        public override void Compose<T>(T bldr, ModuleBuilderRegistry cntx)
        {
            Level = RetrieveBlockValue<int>();
            ILevelModule module = cntx.GetModule<T, ILevelModule>();

            if (module != null)
            {

                module.SetLevel(Level);
                Debug.Log($"Successfuly set the Prerequisite Level to= {Level}");
            }
            else Debug.Log($"Failed set the Prerequisite Level");
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(BLOCK_NODE_OPTION, typeof(int))
                .WithDefaultValue(0)
                .WithDisplayName("Level")
                .Build();
        }
    }
}
