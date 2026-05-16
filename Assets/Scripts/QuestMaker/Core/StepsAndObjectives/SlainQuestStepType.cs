using UnityEngine;
using QuestMaker.Data;

namespace QuestMaker.Runtime.StepsAndObjectives
{
    [CreateAssetMenu(menuName = "QuestMaker/Steps/StepType/Slain")]
    public class SlainQuestStepType : QuestStepTypeSO
    {
        [SerializeField] private string npcID = string.Empty;
        [SerializeField] private ushort Amount = 0;

    }

}