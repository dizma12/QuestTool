using QuestMaker.Editor.Utility;
using UnityEngine;

namespace QuestMaker.Editor.Graph
{
    public class QMGraphAssetFile : ScriptableObject
    {
        [SerializeField] private string guid = string.Empty;

        public string Guid { get => guid; }

        public void SetTargetGUID(string guid)
        {
            if(!string.IsNullOrEmpty(guid))
                this.guid = guid;
        }

    }
}
