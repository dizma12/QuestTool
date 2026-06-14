
using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    public class TalkQuestStep : QuestStep
    {
        private readonly string _npcID = string.Empty;
        private bool _hasTalked = false;
        public override string ProgressText => $"Talk to {_npcID}";

        public override bool IsComplete => Validate();

        public TalkQuestStep(QuestStepData data , IQuestEventSource eventbus) : base(eventbus)
        {

            if (data == null)
            {
                ConsoleLogger.LogError(this, "QuestStepData is null");
                return;
            }
            if (data is not TalkStepData talkData)
            {
                ConsoleLogger.LogError(this, $"Expected TalkStepData, got {data?.GetType()}");
                return;
            }
            if (string.IsNullOrEmpty(talkData.NpcID))
            {
                ConsoleLogger.LogError(this, $"Invalind Talk to npcid with npc: {talkData.NpcID}");
                return;
            }
            _npcID = talkData.NpcID;
        }
        private void HandleTalking(string npcID)
        {
            if(!_npcID.Equals(npcID)) return;

            if (_hasTalked)
            {
                Finish();
                return;
            }

            if (_npcID.Equals(npcID) && !_hasTalked)
            {
                _hasTalked = true;
                FireOnChanged();
                ConsoleLogger.LogWarning(this, $"Finished is not automaticly called for Talk step!");
            }
        }

        protected override bool Validate() => _hasTalked;

        protected override void Subscribe() => _eventBus.OnNpcTalked += HandleTalking;
        protected override void Unsubscribe() => _eventBus.OnNpcTalked -= HandleTalking;
    }
}
