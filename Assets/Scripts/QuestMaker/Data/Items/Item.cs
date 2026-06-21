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
            string assetGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
            bool hasGuid = !string.IsNullOrEmpty(assetGuid);

            // Regenerate when missing, or when the cached id no longer matches the asset's
            // real GUID. A duplicated asset copies _id but gets a fresh GUID, so this is how
            // we detect the copy and give it its own id instead of the original's.
            bool needID = string.IsNullOrEmpty(_id) || (hasGuid && !_id.Equals(assetGuid));

            if (needID)
                // Bind to the asset GUID so the id stays stable across rename/move; only fall
                // back to a generated GUID while the asset has no path yet.
                _id = hasGuid ? assetGuid : GUID.Generate().ToString();

            bool namesMatch = _itemName.Equals(name);
            if (!namesMatch)
                _itemName = name;

            if (needID || !namesMatch)
                EditorUtility.SetDirty(this);
        }
#endif
    }
}