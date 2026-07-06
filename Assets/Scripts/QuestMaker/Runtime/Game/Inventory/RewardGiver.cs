using QuestMaker.Domain;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Quests;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    public class RewardGiver : MonoBehaviour, IRewardGiver
    {
        private QuestEventBus _eventBus = null;
        private Player _player = null;
        private Inventory _inventory = null;

        private void Start()
        {
            _eventBus = ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<QuestEventBus>();
            _player = ReferenceManager.Instance.RequestReference<Player>();
            _inventory = ReferenceManager.Instance.RequestReference<InventoryManager>().Inventory;

            if (_eventBus != null)
                _eventBus.OnQuestCompleted += GrantRewards;
        }
        private void OnDisable()
        {
            if(_eventBus != null)
                _eventBus.OnQuestCompleted -= GrantRewards;
        }
        public void GrantRewards(Quest quest)
        {
            RewardData data = quest.Rewards;
            if (data == null)
            {
                ConsoleLogger.LogWarning(this, $"Quest {quest.ID} does not contain any rewards");
                return;
            }
            HandleItemRewards(data);
            HandleExpRewards(data);

            //Abilities and reputation are Not Implemented
            HandleReputationRewards(data);
            HandleAbilityRewards(data);
        }

        private void HandleItemRewards(RewardData data)
        {
            if (data.Items == null && data.Items.Count <= 0) return;

            foreach (ItemStack item in data.Items)
            {
                if (_inventory.GetItemCount(item.Item) > 0)
                    _inventory.AddItemStack(item);
                else
                    _inventory.AddItem(item);
            }
        }

        private void HandleExpRewards(RewardData data)
        {
            if(data.Exp <= 0) return;

            _player.AddExp(data.Exp);
        }

        private void HandleReputationRewards(RewardData data)
        {
            if(data.Reputation == null || data.Reputation.Count <= 0) return;

            ConsoleLogger.LogWarning(this, $"Rewards for Reputations are not implemented");
        }

        private void HandleAbilityRewards(RewardData data)
        {
            if(data.AbilityIDs == null ||  data.AbilityIDs.Count <= 0) return;

            ConsoleLogger.LogWarning(this, $"Rewards for Abilities are not implemented");
        }
    }
}
