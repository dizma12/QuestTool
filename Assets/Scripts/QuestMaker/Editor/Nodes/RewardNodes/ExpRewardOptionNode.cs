using QuestMaker.Editor.Compiler.CompilationModules;

using System;
using UnityEngine;
using System.Linq;


namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class ExpRewardOptionNode : QMBaseOptionNode, IReputationOptionNode, IRewardOptionNode
    {
        public override Type PortType => typeof(IReputationOptionNode);



        public override void Compose(QuestModuleBuilder cntx)
        {

            var x = this.GetType().GetInterfaces().Where(i => typeof(IOptionNode).IsAssignableFrom(i)).ToList();
            foreach (var i in x)
            {
                Debug.LogWarning($"Count is {x.Count} and element is= {i}");
            }
            //
        }
    }
}
