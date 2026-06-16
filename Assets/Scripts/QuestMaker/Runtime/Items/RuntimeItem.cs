using QuestMaker.Domain;
using UnityEngine;

namespace QuestMaker.Runtime
{
    internal abstract class RuntimeItem : MonoBehaviour
    {
        [SerializeField] protected Item item = null;
    }
}
