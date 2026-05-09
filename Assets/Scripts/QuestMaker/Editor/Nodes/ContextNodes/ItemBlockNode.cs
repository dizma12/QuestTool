using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime;
using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(QMBaseBlockNode))]
    internal class ItemBlockNode : QMBaseBlockNode
    {
        Item item = null;
        public override void Compose(ModuleRegistry cntx)
        {
            item = RetrieveBlockValue<Item>();
            if (item == null)
                Debug.LogError($"[{ContextNode}][ItemBlockNode] Item is Null");

        }
    }
}
