
namespace QuestMaker.Data.SpecialEvents
{
    [System.Serializable]
    public readonly struct SpecialEventData
    {
        public SpecialEventTrigger Trigger { get; }
        public string EventID { get; }

        public SpecialEventData(SpecialEventTrigger trigger, string eventID)
        {
            Trigger = trigger;
            EventID = eventID;
        }
    }
}