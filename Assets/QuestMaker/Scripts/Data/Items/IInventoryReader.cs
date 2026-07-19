namespace QuestMaker.Domain
{
    /// <summary>
    /// Helper interface of inventory that implements GetItemCount(Item).
    /// This was created to avoid Cyclic dependencies, coz steps in Domain assembly needs a reference to Inventory that lives in Runtime.
    /// </summary>
    public interface IInventoryReader
    {
        int GetItemCount(Item item);
    }
}
