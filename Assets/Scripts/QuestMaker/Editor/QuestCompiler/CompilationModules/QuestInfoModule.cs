using QuestMaker.Data;

namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class QuestInfoModule : IQuestModuleBuilder, IQuestInfoModule
    {
        private string questname = string.Empty;
        private string questdesc = string.Empty;

        public void Build(QuestSO quest)
        {
            quest.QuestName = questname;
            quest.QuestDescription = questdesc;
        }

        public void SetQuestDescription(string desc)
        {
            if (string.IsNullOrEmpty(desc)) return;

            questdesc = desc;
        }

        public void SetQuestName(string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            questname = name;
        }
    }
}
