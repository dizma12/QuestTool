using UnityEngine;

namespace QuestMaker.Domain.Interactions
{
    /// <summary>
    /// Interface for interactable objects (quest-Givers, items, etc)
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Callback for When player interacted with the object (passes gameobject -> the player).
        /// </summary>
        /// <param name="interactor"></param>
        void Interact(GameObject interactor);
    }
}

