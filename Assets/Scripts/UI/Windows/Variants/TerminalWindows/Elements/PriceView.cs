using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Variants.TerminalWindows.Elements
{
    public class PriceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _priceText;

        [Space, SerializeField] private Image _priceIcon;

        public void Initialize(string priceText, Sprite priceIcon)
        {
            _priceText.text = priceText;
            _priceIcon.sprite = priceIcon;
        }
    }
}