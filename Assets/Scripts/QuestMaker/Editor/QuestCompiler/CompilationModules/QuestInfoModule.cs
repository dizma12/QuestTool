using QuestMaker.Domain;
using QuestMaker.Domain.SpecialEvents;
using System.Collections.Generic;
using System.Linq;

namespace QuestMaker.Editor.CompilationModules
{
#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.
    internal class QuestInfoModule : IQuestModuleBuilder, IQuestInfoModule, ISpecialEventModule
    {
        private string _qName = string.Empty;
        private string _qDesc = string.Empty;
        private QuestType _qType = QuestType.Hidden;
        private List<SpecialEventData> _specialEvents;
        public void Build(QuestSO quest)
        {
            quest.ID = _qName;
            quest.Description = _qDesc;
            quest.QuestType = _qType;

            if (_specialEvents != null && _specialEvents.Count > 0)
                quest.SpecialEvent = _specialEvents.First();
 
        }

        public void SetQuestDescription(string desc)
        {
            if (string.IsNullOrEmpty(desc)) return;

            _qDesc = desc;
        }

        public void SetQuestName(string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            _qName = name;
        }

        public void SetQuestType(QuestType type)
        {
            if(_qType == type) return;
            _qType = type;
            UnityEngine.Debug.Log($"Quest type was set to= {type}");
        }

        public void SetSpecialEvent(SpecialEventData eventData)
        {
            _specialEvents ??= new List<SpecialEventData>();

            if (_specialEvents.Contains(eventData) || eventData.Equals(default)) return;

            _specialEvents.Add(eventData);
        }
    }
#pragma warning restore CS0618
}
