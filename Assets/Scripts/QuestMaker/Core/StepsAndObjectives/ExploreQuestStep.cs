using QuestMaker.Data.Steps;
using QuestMaker.Runtime.Events.Handlers;
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using QuestMaker.Runtime.StepsAndObjectives;
using System;
using UnityEngine;

namespace QuestMaker.Core
{
    internal class ExploreQuestStep : QuestStep
    {
        private string _areaID = string.Empty;
        private bool _isExplored = false;
        private GameEventHandler _eventHandler = null;
        public ExploreQuestStep(QuestStepData data) : base(data) { }


        public override void Initialize(QuestStepData data)
        {
            if (data == null)
            {
                Debug.LogError($"[ExploreQuestStep] QuestStepData is null");
                return;
            }
            if (data is not ExploreStepData exploreData)
            {
                Debug.LogError($"[ExploreQuestStep] Expected ExploreStepData, got {data?.GetType()}");
                return;
            }

            _areaID = exploreData.AreaID;

            _eventHandler = ReferenceManager.Instance
                .GetReference<GameEventManager>()
                .GetEventHandler<GameEventHandler>();

            _eventHandler.OnAreaEntered += HandleAreaEntered;
        }

        private void HandleAreaEntered(string areaID)
        {
            if (_isExplored)
            {
                FinishStep();
                return;
            }

            if (_areaID.Equals(areaID) && !_isExplored)
            {
                _isExplored = true;
                _eventHandler.OnAreaEntered -= HandleAreaEntered;
                FinishStep();
            }
        }

        public override bool Validate() => _isExplored;

    }
}