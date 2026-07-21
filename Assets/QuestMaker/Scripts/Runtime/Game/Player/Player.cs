using QuestMaker.Domain.Helpers;
using QuestMaker.Runtime.Events;
using System;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    /// <summary>
    /// Note-> even Player is not a singleton Player should only be instanciated once.
    /// </summary>
    internal class Player : MonoBehaviour, IGameReference
    {
        //consts

        private const float _nextLevelModifier = RuntimeSettings.NEXT_LEVEL_MODIFIER;

        //public getters
        public ushort Level => _level;
        public string ExpProgress => $"{_currentExp}/{_expToNextLevel}";
        //inspector
        [SerializeField, ReadOnlyInspector]
        private ushort _level = 1;
        [SerializeField, ReadOnlyInspector]
        private uint _currentExp = 0;
        [SerializeField, ReadOnlyInspector]
        private uint _expToNextLevel = RuntimeSettings.STARTING_NEXT_LEVEL_EXP;


        //private
        private PlayerEventBus _playerBus = null;

        private void OnEnable()
        {
            SubscribeSelf();
        }
        private void OnDisable()
        {
            UnsubscribeSelf();
        }

        private void Start()
        {
            _playerBus ??= ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<PlayerEventBus>();
        }

        //Used with Quantom console Depricated now
        public void AddExp(int expToAdd)
        {
            uint positiveExpToAdd = Convert.ToUInt32(Mathf.Abs(expToAdd));

            if (_level == RuntimeSettings.MAX_PLAYER_LEVEL)
            {
                if (_currentExp < _expToNextLevel)
                    _currentExp = _expToNextLevel;
                return;
            }

            if (_currentExp + positiveExpToAdd < _expToNextLevel)
                _currentExp += positiveExpToAdd;
            else
            {
                uint difference = (_currentExp + positiveExpToAdd) - _expToNextLevel;

                //Next Level
                //https://stackoverflow.com/questions/39904658/convert-float-to-ushort

                _expToNextLevel *= Convert.ToUInt32(_nextLevelModifier);

                _level++;

                if (_level == RuntimeSettings.MAX_PLAYER_LEVEL)
                {
                    _currentExp = _expToNextLevel;
                    return;
                }

                _currentExp = difference;

                if (_currentExp >= _expToNextLevel)
                    AddExp(0);
                else
                    _playerBus.FireOnPlayerLevelChanged();

            }
        }

        //Used with Quantom console Depricated now
        public void ResetLevel()
        {
            _level = 1;
            _currentExp = 0;
            _expToNextLevel = 200;
        }

        private bool SubscribeSelf()
            => ReferenceManager.Instance.SubScribeReference<Player>(this);

        private bool UnsubscribeSelf()
        {
            if (ReferenceManager.Instance != null)
                return ReferenceManager.Instance.UnsubscribeReference<Player>();

            return false;
        }
    

    }
}
