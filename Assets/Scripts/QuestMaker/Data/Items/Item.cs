using QuestMaker.Domain.Helpers;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace QuestMaker.Domain
{
    [CreateAssetMenu(menuName = "QuestMaker/ScriptableObjects/Item")]
    [Serializable]
    public class Item : ScriptableObject
    {
        public string ID => _id;
        public string Name => _itemName;
        public GameObject Prefab => _prefab;

        [SerializeField, ReadOnlyInspector] private string _id = string.Empty;
        [SerializeField, ReadOnlyInspector] private string _itemName = string.Empty;
        [SerializeField] private GameObject _prefab = null;

#if UNITY_EDITOR
        private void OnEnable()
        {
            bool needID = string.IsNullOrEmpty(_id);

            if (needID)
            {
                _id = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));

                // id can be null if the path is not yet created by unity
                if (string.IsNullOrEmpty(_id))
                    _id = GUID.Generate().ToString();
            }

            bool namesMatch = _itemName.Equals(name);
            if (!namesMatch)
            {
                _itemName = name;
            }

            if(needID || !namesMatch) 
                EditorUtility.SetDirty(this);
        }
#endif
    }
}