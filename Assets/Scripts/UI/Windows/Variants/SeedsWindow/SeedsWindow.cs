using UI.ElementCard.Slot;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUp.Variants.Garden.Windows
{
    public class SeedsWindow : AbstractWindow
    {
        [Header("Seeds elements")]
        [SerializeField] private DropSlot _seedDropSlot;

        [Space, SerializeField] private Button _growButton;

        [Header("Growing elements")] 
        [SerializeField] private Slider _growingProgressSlider;

        [Space, SerializeField] private Button _peakButton;
        
        
    }
}