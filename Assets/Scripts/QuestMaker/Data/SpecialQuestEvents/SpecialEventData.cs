using UnityEditor;
using UnityEngine;

namespace QuestMaker.Domain.SpecialEvents
{
    [System.Serializable]
    public class SpecialEventData
    {
        public SpecialEventTrigger Trigger => _trigger;
        public string EventID => _eventID;

        [SerializeField]
        private string _eventID;

        [SerializeField]
        private SpecialEventTrigger _trigger;
        public SpecialEventData(SpecialEventTrigger trigger, string eventID)
        {
            _trigger = trigger;
            _eventID = eventID;
        }
    }

}

