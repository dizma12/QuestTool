using UnityEngine;


namespace QuestMaker.Domain
{
    /// <summary>
    /// Struct that containts Item and amount. Default constructor creates ItemStack = default. 
    /// *** USE CONSTRUCTOR WITH PARAMETERS *** 
    /// </summary>
    [System.Serializable] 
    public struct ItemStack
    {
        [SerializeField] private  Item _item;
        [SerializeField] private  int _amount;
        public readonly Item Item => _item;
  
        public readonly int Amount => _amount;


        public ItemStack(Item item, int amount)
        {
            _item = item;
            _amount = amount;
        }
    }
}