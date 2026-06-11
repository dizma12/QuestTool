using UnityEngine;

namespace QuestMaker.Data.SpecialEvents
{
    [System.Serializable]
    public struct SpecialEventData
    {
        public readonly SpecialEventTrigger Trigger => _trigger;
        public readonly string EventID => _eventID;

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