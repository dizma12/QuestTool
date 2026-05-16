using UnityEngine;
using System;
namespace QuestMaker.Runtime.Game
{
    public interface IGameReference
    {

        bool SubscribeSelf();
 
        bool UnsubscribeSelf();
    }
}