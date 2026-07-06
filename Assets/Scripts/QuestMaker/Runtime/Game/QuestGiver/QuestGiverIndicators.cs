using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    public class QuestGiverIndicators : MonoBehaviour
    {
        [SerializeField] private Transform _root = null;
        [SerializeField] private Transform _whiteQuestionMark = null;
        [SerializeField] private Transform _yellowQuestionMark = null;
        [SerializeField] private Transform _whiteExclamationMark = null;
        [SerializeField] private Transform _yellowExclamationMark = null;

        /// <summary>
        /// Activates Yellow QuestionMark indicator.
        /// </summary>
        public void ActivateHandInIndicator()
        {
            if(!_root.gameObject.activeInHierarchy)
                _root.gameObject.SetActive(true);

            _yellowExclamationMark.gameObject.SetActive(true);
            _whiteExclamationMark.gameObject.SetActive(false);

            _yellowQuestionMark.gameObject.SetActive(false);
            _whiteQuestionMark.gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates Yellow ExclamationMark indicator.
        /// </summary>
        public void ActivateTurnInIndicator()
        {
            if (!_root.gameObject.activeInHierarchy)
                _root.gameObject.SetActive(true);

            _yellowExclamationMark.gameObject.SetActive(false);
            _whiteExclamationMark.gameObject.SetActive(false);

            _yellowQuestionMark.gameObject.SetActive(true);
            _whiteQuestionMark.gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates White ExclamationMark Indicator.
        /// </summary>
        public void ActivateMissingReqHandInIndicator()
        {
            if (!_root.gameObject.activeInHierarchy)
                _root.gameObject.SetActive(true);

            _yellowExclamationMark.gameObject.SetActive(false);
            _whiteExclamationMark.gameObject.SetActive(true);

            _yellowQuestionMark.gameObject.SetActive(false);
            _whiteQuestionMark.gameObject.SetActive(false);
        }

        public void ActivateInProgressTurnInIndicator()
        {
            if (!_root.gameObject.activeInHierarchy)
                _root.gameObject.SetActive(true);

            _yellowExclamationMark.gameObject.SetActive(false);
            _whiteExclamationMark.gameObject.SetActive(false);

            _yellowQuestionMark.gameObject.SetActive(false);
            _whiteQuestionMark.gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivates Root Indicator GameObject.
        /// </summary>
        public void DeactivateIndicators()
        {
            _yellowExclamationMark.gameObject.SetActive(false);
            _whiteExclamationMark.gameObject.SetActive(false);

            _yellowQuestionMark.gameObject.SetActive(false);
            _whiteQuestionMark.gameObject.SetActive(false);

            if (_root.gameObject.activeInHierarchy)
                _root.gameObject.SetActive(false);
        }
    }
}
