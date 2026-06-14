using System;
using UnityEngine;


namespace QuestMaker.Domain
{
    [Serializable]
    public struct ReputationFaction
    {
        [SerializeField] private string _factionID;
        [SerializeField] private int _amount;
        public string FactionID
        {
            readonly get => _factionID;
            init
            {
                if (value != null)
                    _factionID = value;
            }
        }
        public int Amount
        {
            readonly get => _amount;
            init
            {
                if (_amount != value && value > 0)
                    _amount = value;
            }
        }
    }
}
