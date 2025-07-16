using System;
using Player;
using ProcessBuilding.Garden;
using Sounds;
using UI.Effects.ScalingEffect;
using UI.PopUp;
using UI.PopUp.Variants.Garden;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Utiles.EventSystem;
using Zenject;

public class DestroyWindow : AbstractWindow
{
    #region Buttons

    [Header("Buttons")]
    [SerializeField] private Button _destroyButton;
    [SerializeField] private Button _cancelBatton;
    [SerializeField] private string _destroySound;

    #endregion

    [Header("Effects")]
    [SerializeField] private VisualEffect _destroyEffect;

    private SoundService _soundService;

    private GameObject _root;
    private AbstractPopUp _gardenPopUp;

    private void Awake()
    {
        _soundService = SoundService.Instance;
    }

    public void Initialize(GameObject root, AbstractPopUp popUp)
    {
        _root = root;
        _gardenPopUp = popUp;
    }
    private void OnEnable()
    {
        if (_destroyButton is not null)
        {
            _destroyButton.onClick.AddListener(DestroyBuild);
        }

         if(_cancelBatton is not null)
         {
            _cancelBatton.onClick.AddListener(CloseAction);
         }

    }
    private void CloseAction()
    {
        Close();
    }

    private void DestroyBuild()
    {
        Instantiate(_destroyEffect, _root.transform.position, Quaternion.identity).Play();
        _soundService.Play(SoundType.Music, _destroySound);

        EventBus.Instance.Publish<StopAction>(new StopAction());
        Destroy(_root);

    }
}
