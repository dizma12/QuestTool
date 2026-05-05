using QuestMaker.Runtime.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
namespace QuestMaker.Editor.Nodes
{
    internal interface IHubNodeCollector<out T> where T : QuestData
    {
        T Collect();
    }

    [Serializable]
    internal class QMRewardHubNode : QMBaseHubNode, IHubNodeCollector<RewardData>
    {
        public override Type PortType => typeof(RewardData);


        public RewardData Collect()
        {

            throw new NotImplementedException();
            ;


        }
    }
}
