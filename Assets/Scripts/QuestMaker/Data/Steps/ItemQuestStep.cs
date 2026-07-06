using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    public abstract class ItemQuestStep : QuestStep
    {
        protected ItemStack _questItem = default;
        protected int _currentAmount = 0;

        public ItemStack QuestItem => _questItem;

        public override bool IsComplete { get; protected set; } = false;

        protected ItemQuestStep(IQuestEventSource eventbus) : base(eventbus)
        {
        }

        protected void HandleItemProgress(ItemStack stack)
        {
            if (_questItem.Item == null || !_questItem.Item.ID.Equals(stack.Item.ID))
                return;

            _currentAmount += stack.Amount;
            FireOnChanged();

            if (Validate())
                Finish();
        }

        public virtual void CheckInventoryForExisting(IInventoryReader inventory)
        {
            if (inventory == null || IsComplete || _questItem.Item == null) return;

            int owned = inventory.GetItemCount(_questItem.Item);
            if (owned <= _currentAmount) return;

            _currentAmount = owned >= _questItem.Amount ? _questItem.Amount : owned;
            FireOnChanged();

            if (Validate())
                Finish();
        }

        protected override bool Validate() => _currentAmount >= _questItem.Amount;
    }
}
