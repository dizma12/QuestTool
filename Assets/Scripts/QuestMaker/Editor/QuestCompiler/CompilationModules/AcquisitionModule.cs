using QuestMaker.Domain;
using QuestMaker.Domain.Quests;
using QuestMaker.Editor.CompilationModules;


namespace QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.QuestCompiler.CompilationModules
{
    internal class AcquisitionModule : IQuestModuleBuilder, IAcquisitionModule
    {
        private QuestGiverData _acquisition = null;
        private QuestGiverData _turnIn = null;
        
        public void Build(QuestSO quest)
        {
            if (_acquisition != null)
            {
                _acquisition.AddHandInQuest(quest);
                quest.HandInMethod = _acquisition;
            }

            if (_turnIn != null)
            {
                _turnIn.AddTurnInQuest(quest);
                quest.TurnInMethod = _turnIn;
            }
        }

        public void SetAcquisitionMethod(QuestGiverData data)
        {
            if (data == null) return;
            _acquisition = data;
        }

        public void SetTurnInMethod(QuestGiverData data)
        {
            if(data == null) return;
            _turnIn = data;
        }
    }
}
