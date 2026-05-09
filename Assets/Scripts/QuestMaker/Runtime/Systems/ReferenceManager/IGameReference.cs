using UnityEngine;
using System;
namespace Game.Runtime.Managers
{
    public interface IGameReference
    {

        bool SubscribeSelf();
 
        bool UnsubscribeSelf();
    }
}