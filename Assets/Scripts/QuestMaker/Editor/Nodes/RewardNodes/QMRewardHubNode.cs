using QuestMaker.Runtime.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
namespace QuestMaker.Editor.Nodes
{


    [Serializable]
    internal class QMRewardHubNode : QMBaseHubNode
    {
        public override Type PortType => typeof(RewardData);
    }
}
