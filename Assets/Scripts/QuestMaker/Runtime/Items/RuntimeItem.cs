using QuestMaker.Domain;
using QuestMaker.Domain.Helpers;
using UnityEngine;

namespace QuestMaker.Runtime
{
    internal abstract class RuntimeItem : MonoBehaviour
    {
        [SerializeField] protected Item item = null;
        [SerializeField, ReadOnlyInspector] protected string ID = string.Empty;
    }
}
