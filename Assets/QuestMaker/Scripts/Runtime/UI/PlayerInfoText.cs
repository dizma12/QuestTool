using QuestMaker.Domain;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using System;
using TMPro;
using UnityEngine;

namespace QuestMaker.Runtime.UI
{
    public class PlayerInfoText : MonoBehaviour
    {
        [SerializeField] TMP_Text _display = null;
        private Player _player = null;
        private PlayerEventBus _playerEventBus = null;
        private string _displayText => $"Level: {_player.Level} Exp: {_player.ExpProgress}";
        private void Start()
        {
            if(_display == null)
            {
                ConsoleLogger.LogError(this, "Player info text is null. Please check Scene Object");
                return;
            }
            _player = ReferenceManager.Instance.RequestReference<Player>();

            if (_player == null)
            {
                ConsoleLogger.LogError(this, "Failed to find Player reference");
                return;
            }
            _playerEventBus = ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<PlayerEventBus>();

            if(_playerEventBus == null)
            {
                ConsoleLogger.LogError(this, "Failed to find Player event bus reference");
                return;
            }
            _playerEventBus.PlayerExpChanged += ChangePlayerInfoText;
            _playerEventBus.PlayerLevelChanged += ChangePlayerInfoText;

            ChangePlayerInfoText();
        }

        private void ChangePlayerInfoText() => _display.text = _displayText;

    }
}
