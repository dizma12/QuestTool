using QuestMaker.Domain.Interactions;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    [RequireComponent(typeof(CircleCollider2D))]
    internal class PlayerInteractions : MonoBehaviour
    {

        private readonly List<IInteractable> _inRange = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                InteractNearest();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable) && !_inRange.Contains(interactable))
            {
                _inRange.Add(interactable);
    
                return;
            }
            if (other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _inRange.Remove(interactable);
 
            }
        }

        private void InteractNearest()
        {
            IInteractable nearest = null;
            float bestDistance = float.MaxValue;

            foreach (IInteractable interactable in _inRange)
            {
                //Cant get transform from interactable and cant check for GO so we check for component.
                if (interactable is not Component component) continue;

                //using sqrMagnituted based on Unity's recommendation since we dont need the actual distance
                //https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Vector3-sqrMagnitude.html

                float posDif = ((Vector2)transform.position - (Vector2)component.transform.position).sqrMagnitude;
                if (posDif < bestDistance)
                {
                    bestDistance = posDif;
                    nearest = interactable;
                }
            }

            nearest?.Interact(gameObject);
        }
    }
}