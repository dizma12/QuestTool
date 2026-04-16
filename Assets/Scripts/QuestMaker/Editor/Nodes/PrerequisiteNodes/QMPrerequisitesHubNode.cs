using System;
using Codice.CM.SEIDInfo;
using QuestMaker.Runtime.Data;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    internal class QMPrerequisitesHubNode : QMBaseHubNode
    {
        public const string PREREQ_OUTPUT_PORT = "Prereq_Output_Port";
        public override Type PortType { get => typeof(PrerequisiteData); }

    }
}
