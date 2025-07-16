using ProcessBuilding.Garden;
using Sounds;
using UI.Effects.ScalingEffect;
using UI.PopUp.Variants.Garden;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Zenject;

public class DestroyWindow : AbstractWindow
{
    #region Buttons

    [Header("Buttons")]
    [SerializeField] private Button _destroyButton;
    [SerializeField] private Button _cancelBatton;

    #endregion

    [Header("Effects")]
    [SerializeField] private VisualEffect _destroyEffect;

    [Inject] private SoundService _soundService;

    private ProcessBuilding.Garden.Garden _garden;
    private GardenPopUp _gardenPopUp;

    public void Initialize(ProcessBuilding.Garden.Garden garden, GardenPopUp popUp)
    {
        _garden = garden;
        _gardenPopUp = popUp;
    }
    private void OnEnable()
    {

        if (_destroyButton is not null)
        {
            _destroyButton.onClick.AddListener(DestroyGarden);
        }

         if(_cancelBatton is not null)
         {
            _cancelBatton.onClick.AddListener(CloseAction);
         }

    }
    private void CloseAction()
    {
        _gardenPopUp.CloseDestroyWindow();
    }

    private void DestroyGarden()
    {
        Close();
        _gardenPopUp.Close();
        Instantiate(_destroyEffect, _garden.transform.position, Quaternion.identity).Play();
        _soundService.Play(SoundType.Music, "destroyGarden");
        Destroy(_garden.gameObject);

    }
}
