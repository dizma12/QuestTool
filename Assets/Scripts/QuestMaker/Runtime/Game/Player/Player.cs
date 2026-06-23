using QFSW.QC;
using QuestMaker.Domain;
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
        public const ushort MAX_LEVEL = 20;
        private const float _nextLevelModifier = 1.5f;

        //public getters
        public uint Level => _level;
        public Inventory Inventory { get; private set; } = null;

        //inspector
        [SerializeField, ReadOnlyInspector]
        private uint _level = 1;
        [SerializeField, ReadOnlyInspector]
        private uint _currentExp = 0;
        [SerializeField, ReadOnlyInspector] // by current formula u need 886.062 exp to reach max level (20);
        private uint _expToNextLevel = 200;


        //private
        private PlayerEventBus _playerBus = null;

        private void OnEnable()
        {
            Inventory ??= new();
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

        [Command()]
        public void AddExp(int expToAdd)
        {
            if (_level == MAX_LEVEL)
            {
                if (_currentExp < _expToNextLevel)
                    _currentExp = _expToNextLevel;
                return;
            }

            if (expToAdd < 0)
            {
                ConsoleLogger.LogError(this, "Exp to add cannot be <= 0");
                return;
            }
            if (_currentExp + expToAdd < _expToNextLevel)
                _currentExp += (uint)expToAdd;
            else
            {
                uint difference = (uint)(_currentExp + expToAdd) - _expToNextLevel;

                //Next Level
                //https://stackoverflow.com/questions/39904658/convert-float-to-ushort
                uint expToNext = Convert.ToUInt32(_expToNextLevel * _nextLevelModifier);
                _expToNextLevel = expToNext;
                _level++;

                if (_level == MAX_LEVEL)
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

        [Command()]
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
