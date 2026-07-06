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
    internal class TurnInBlockNode : QMBaseBlockNode
    {
        public const string TURN_IN_OPTION = "TURN_IN_OPTION";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(TURN_IN_OPTION, typeof(QuestGiver))
                .WithDefaultValue(null)
                .WithDisplayName("Turn-in Giver")
                .WithTooltip("*** NEED TO MANUALLY DRAG THE PREFAB TO SLOT CAN'T FIND IT FROM UNITY SEARCH ***")
                .Build();
        }

        public override void Compose(ModuleScope scope)
        {
            QuestGiver giver = RetrieveBlockValue<QuestGiver>(TURN_IN_OPTION);

            if (giver == null)
            {
                ConsoleLogger.LogError(this, "Turn-In giver is null");
                return;
            }

            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(giver));
            scope.Get<IAcquisitionModule>().SetTurnInMethod(guid);
        }
    }
}
