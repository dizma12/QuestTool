using QuestMaker.Domain;
using QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.QuestCompiler.CompilationModules;
using QuestMaker.Editor.CompilationModules;
using QuestMaker.Runtime.Game;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace QuestMaker.Editor.Nodes
{
    [UseWithContext(typeof(AcquisitionContextNode))]
    [System.Serializable]
    internal class AcquisitionBlockNode : QMBaseBlockNode
    {
        public const string ACQUISITION_OPTION = "ACQUISITION_OPTION";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(ACQUISITION_OPTION, typeof(QuestGiver))
                .WithDefaultValue(null)
                .WithDisplayName("Hand-in Giver")
                .WithTooltip("*** NEED TO MANUALLY DRAG THE PREFAB TO SLOT CAN'T FIND IT FROM UNITY SEARCH ***")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            QuestGiver giver = RetrieveBlockValue<QuestGiver>(ACQUISITION_OPTION);

            if (giver == null)
            {
                ConsoleLogger.LogError(this, "Acquisition giver is null");
                return;
            }

            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(giver));
            scope.Get<IAcquisitionModule>().SetAcquisitionMethod(guid);
        }
    }
}
