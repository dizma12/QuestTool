using UnityEngine;
using QuestMaker.Data.StepsAndObjectives;

namespace QuestMaker.Runtime.StepsAndObjectives
{
    [CreateAssetMenu(menuName = "QuestMaker/Steps/StepType/Slain")]
    public class SlainQuestStepType : QuestStepSO
    {
        [SerializeField] private string npcID = string.Empty;
        [SerializeField] private ushort Amount = 0;

    }

}