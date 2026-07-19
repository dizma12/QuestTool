using QuestMaker.Domain.Events;

namespace QuestMaker.Domain.Steps
{
    public abstract class ItemQuestStep : QuestStep
    {
     
        protected ItemStack _questItem = default;
        protected int _currentAmount = 0;

        /// <summary>
        /// The item that is required for the quest / the total amount required for the quest. AMOUNT is TOTAL amount NOT the current Amount 
        /// </summary>
        public ItemStack QuestItem => _questItem;

        public override bool IsComplete { get; protected set; } = false;

        /// <summary>
        /// The Current amount owned of the required item.
        /// </summary>
        public int CurrentAmount => _currentAmount;
        protected ItemQuestStep(IQuestEventSource eventbus) : base(eventbus) { }
       

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
            //this method runs on step creation so _currentAmount is 0.
            if (inventory == null || IsComplete || _questItem.Item == null) return;

            int owned = inventory.GetItemCount(_questItem.Item);
            if (owned <= _currentAmount) return;

            //sets the current amount based on the inventory.
            _currentAmount = owned >= _questItem.Amount ? _questItem.Amount : owned;
            FireOnChanged();

            if (Validate())
                Finish();
        }

        protected override bool Validate() => _currentAmount >= _questItem.Amount;
    }
}
