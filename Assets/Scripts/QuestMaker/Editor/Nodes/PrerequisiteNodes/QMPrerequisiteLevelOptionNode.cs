using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime.Data;
using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;
namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMPrerequisiteLevelOptionNode : QMBaseOptionNode
    {
        public int Level = 0;

        public override Type PortType { get => typeof(IPrerequisiteOptionNode); }

        public override void Compose(ModuleRegistry cntx)
        {

            //Level = ReadNodeOption<int>();

            //Debug.Log($"Composing From Node= {GetType().Name}");
            //cntx.GetModule<PrerequisiteModule>().SetLevelPrerequisite(Level);
        }

        //public override void Compose(QuestCompilationContext cntx)
        //{
        //    var preq = new LevelPrerequisiteData
        //    {
        //        Level = this.Level
        //    };
        //    cntx.AddQuestPrerequisite(preq);

        //}

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OPTION_NODE_PORT, typeof(int))
                .WithDefaultValue(default)
                .WithDisplayName("Level")
                .Build();
        }

    }
}
