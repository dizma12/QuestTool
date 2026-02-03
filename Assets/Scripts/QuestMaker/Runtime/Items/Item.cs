using UnityEngine;

namespace QuestMaker.Runtime
{
    public class Item
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        public Item(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}