using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;


namespace QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.QuestCompiler.CompilationModules
{
    internal class AcquisitionModule : IQuestModuleBuilder, IAcquisitionModule
    {
        private string _handInGuid = null;
        private string _turnInGuid = null;

        public void Build(QuestSO quest)
        {
            if (!string.IsNullOrEmpty(_handInGuid)) quest.SetHandInGiver(_handInGuid);
            if (!string.IsNullOrEmpty(_turnInGuid)) quest.SetTurnInGiver(_turnInGuid);
        }

        public void SetAcquisitionMethod(string giverGuid)
        {
            if (string.IsNullOrEmpty(giverGuid)) return;
            _handInGuid = giverGuid;
        }

        public void SetTurnInMethod(string giverGuid)
        {
            if (string.IsNullOrEmpty(giverGuid)) return;
            _turnInGuid = giverGuid;
        }
    }
}
