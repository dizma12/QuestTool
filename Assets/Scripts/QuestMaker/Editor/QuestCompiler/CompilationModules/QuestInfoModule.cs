using QuestMaker.Data;

namespace QuestMaker.Editor.Compiler.CompilationModules
{
#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.
    internal class QuestInfoModule : IQuestModuleBuilder, IQuestInfoModule
    {
        private string _qName = string.Empty;
        private string _qDesc = string.Empty;
        private QuestType _qType = QuestType.Hidden;

        public void Build(QuestSO quest)
        {
            quest.QuestName = _qName;
            quest.Description = _qDesc;
            quest.QuestType = _qType;
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
    }
#pragma warning restore CS0618
}
