using QuestMaker.Runtime.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaker.Editor.Nodes
{
    internal class QMPrerequisiteLevelOptionNode : QMBaseOptionNode
    {
        public int Level = 0;

        public override Type PortType { get => typeof(PrerequisiteData); }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(OPTION_NODE_PORT, PortType)
                .WithDefaultValue(default)
                .WithDisplayName(OPTION_NODE_INPUT_DISPLAY_NAME)
                .Build();
        }
    }
}
