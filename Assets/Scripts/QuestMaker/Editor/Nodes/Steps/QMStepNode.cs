using System;
using QuestMaker.Runtime.StepsAndObjectives;
using QuestMaker.Editor.Compiler.CompilationModules;
using UnityEditor.Experimental.GraphView;
using System.Diagnostics;
using UnityEngine;
namespace QuestMaker.Editor.Nodes.Steps
{
    [Serializable]
    internal class QMStepNode : QMBaseOptionNode
    {
        public override Type PortType => typeof(IOptionNode);

        public QuestStepTypeSO stepType = null;

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OPTION_NODE_PORT, typeof(QuestStepTypeSO))
                   .WithDefaultValue(null)
                   .WithDisplayName("Quest Type")
                   .Build();

        }

        public override void Compose(ModuleRegistry cntx)
        {
            UnityEngine.Debug.LogError("Not Implemented");
        }
    }
    internal class QMStepViewNode : GraphElement
    {
        
    
    }

}
