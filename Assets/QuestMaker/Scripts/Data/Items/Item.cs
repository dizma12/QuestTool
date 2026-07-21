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
        public bool IsExclusivelyQuestItem => _isExclusivelyQuestItem;
        public GameObject Prefab => _prefab;
        public Sprite Display => _display;

        [Tooltip("Name is set automaticaly from .asset name")]
        [SerializeField, ReadOnlyInspector] private string _id = string.Empty;

        [Tooltip("ID is set automaticaly from GUID")]
        [SerializeField, ReadOnlyInspector] private string _itemName = string.Empty;

        [Tooltip("If this is checked means the item is only used for Quests. This means when completing a quest all stacks will be removed"
                + "Even if the current amount is more than needed (ex: 11/10 -> 11 will be removed instead of 10")]
        [SerializeField] private bool _isExclusivelyQuestItem = false;
        [SerializeField] private Sprite _display = null;
        [SerializeField] private GameObject _prefab = null;

#if UNITY_EDITOR
        private void OnEnable()
        {
            string assetGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
            bool hasGuid = !string.IsNullOrEmpty(assetGuid);


            // checks if the id is empty or the GUID does not match the actual GUID
            // if it doesnt match means item was duplicated and needs a new one.
            bool needID = string.IsNullOrEmpty(_id) || (hasGuid && !_id.Equals(assetGuid));

            if (needID) // some times this runs before unity generates the asset guid if its newly created.
                _id = hasGuid ? assetGuid : GUID.Generate().ToString(); // if this is the case we generate a new one manually
            
            //Items name is set automaticly from assets name.
            bool namesMatch = _itemName.Equals(name);
            if (!namesMatch)
                _itemName = name;

            if (needID || !namesMatch)
                EditorUtility.SetDirty(this);
        }
#endif
    }
}