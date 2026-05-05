using System;
using Codice.CM.SEIDInfo;
using QuestMaker.Runtime.Data;
using Unity.GraphToolkit.Editor;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMPrerequisitesHubNode : QMBaseHubNode
    {
        public override Type PortType { get => typeof(PrerequisiteData); }

    }
}
