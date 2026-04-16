using System;
using UnityEngine;
using UnityEditor;

namespace QuestMaker.Runtime
{
    [CreateAssetMenu(menuName = "QuestMaker/Item")]
    [Serializable]
    public class Item : ScriptableObject
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        [SerializeField] private string id;
        [SerializeField] private string itemName;

        public Item(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}