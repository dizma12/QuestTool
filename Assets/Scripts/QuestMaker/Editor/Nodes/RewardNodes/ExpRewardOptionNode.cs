using QuestMaker.Editor.Compiler.CompilationModules;

using System;
using UnityEngine;
using System.Linq;


namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class ExpRewardOptionNode : QMBaseOptionNode, IReputationOptionNode, IRewardOptionNode
    {
        public override Type PortType => typeof(IOptionNode);

        public override void Compose(ModuleRegistry cntx)
        {
            ILevelModule levelModule = cntx.GetModule<PrerequisiteModule, ILevelModule>();
            if (levelModule == null)
            {
                Debug.Log("Null mode correct type");
                return;
            }
            levelModule.SetLevel(3);

            INpcModule npcModule = cntx.GetModule<PrerequisiteModule, INpcModule>();
            if(npcModule == null)
            {
                Debug.Log("Null mode with wrong type");
            }
        }
    }
}
