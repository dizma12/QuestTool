using System;
namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMPrerequisitesHubNode : QMBaseHubNode
    {
        public override Type PortType { get => typeof(IOptionNode); }

    }
}
