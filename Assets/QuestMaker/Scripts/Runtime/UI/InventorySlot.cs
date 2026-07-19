using QuestMaker.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestMaker.Runtime
{
    public class InventorySlot : MonoBehaviour
    {

        [SerializeField] private Image _image = null;
        [SerializeField] private TMP_Text _text = null;
        private void Start()
        {
            if (_image == null)
            {
                if (!TryGetComponent(out _image))
                    ConsoleLogger.LogError(this, "Failed to find Component of type Image");

                gameObject.SetActive(false);
                return;
            }

            if (_text == null)
            {
                _text = GetComponentInChildren<TMP_Text>();
                if (_text == null)
                {
                    ConsoleLogger.LogError(this, "Failed to find Component in children of type TMP_Text");
                    gameObject.SetActive(false);
                }
            }

        }
        public void PrepareInventorySlot(Sprite imageToDisplay, int stackAmount)
        {
            if (imageToDisplay == null) return;

            if (stackAmount == 0) return;
            if(stackAmount < 0) stackAmount = Mathf.Abs(stackAmount);

            _image.sprite = imageToDisplay;
            _text.text = stackAmount.ToString();

        }
    }
}
