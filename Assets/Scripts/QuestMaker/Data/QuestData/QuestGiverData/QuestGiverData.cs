using QuestMaker.Domain.Helpers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestMaker.Domain.Quests
{
    [CreateAssetMenu(menuName = "QuestMaker/ScriptableObjects/QuestGiverData")]
    public class QuestGiverData : ScriptableObject
    {
        [Tooltip("Npc id is automaticly set from Scriptable objects Name")]
        [SerializeField, ReadOnlyInspector] private string _npcId = string.Empty;

        [Tooltip("Hand-In quests are set by graph Aqcuisition Context Node")]
        [SerializeReference, ReadOnlyInspector] private List<QuestSO> _handInQuests = new();

        [Tooltip("Turn-In quests are set by graph Aqcuisition Context Node")]
        [SerializeReference, ReadOnlyInspector] private List<QuestSO> _turnInQuests = new();
       
        public string ID => _npcId;
        public IReadOnlyList<QuestSO> HandInQuests => _handInQuests;
        public IReadOnlyList<QuestSO> TurnInQuests => _turnInQuests;


#if UNITY_EDITOR
        public void AddHandInQuest(QuestSO quest)
        {
            if(quest == null || _handInQuests.Contains(quest)) return;
            _handInQuests.Add(quest);
            EditorUtility.SetDirty(this);

        }
        public void AddTurnInQuest(QuestSO quest)
        {
            if (quest == null || _turnInQuests.Contains(quest)) return;
            _turnInQuests.Add(quest);
            EditorUtility.SetDirty(this);
        }
        public void OnEnable()
        {
            if (!_npcId.Equals(name))
            {
                _npcId = name;
                EditorUtility.SetDirty(this);
            }

            for (int i = 0; i < _handInQuests.Count; i++)
            {
                if (_handInQuests[i] == null)
                    _handInQuests.RemoveAt(i);
            }
            for (int i = 0; i < _turnInQuests.Count; i++)
            {
                if (_turnInQuests[i] == null)
                    _turnInQuests.RemoveAt(i);
            }
        }
#endif
    }


}
