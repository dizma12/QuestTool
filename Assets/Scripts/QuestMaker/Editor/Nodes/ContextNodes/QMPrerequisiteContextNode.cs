using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMPrerequisiteContextNode : QMBaseContextNode
    {

        public override Type PortType => typeof(IOptionNode);

        //public override TBuilder GetBuilder<TBuilder>(ModuleBuilderRegistry reg)
        //{
        //    return reg.GetBuilder<TBuilder>();
        //}

        public override Type GetBuilder() => typeof(PrerequisiteModule);


        public override bool ProccessNodes(ModuleBuilderRegistry reg)
        {
            var builder = reg.GetBuilder<PrerequisiteModule>();
            if(builder == null) return false;

            IComposableNode[] nodes = GetBlockNodes();

            foreach(IComposableNode node in nodes)
            {
                node.Compose(builder, reg);
            }
            return true;
        }
    }
}
