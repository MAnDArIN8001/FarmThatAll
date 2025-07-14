using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private string _sceneName;
    [SerializeField] private SceneFader _sceneFader;

    private void OnEnable()
    {
        if(_playButton is not null)
        {
            _playButton.onClick.AddListener(FadeAndLoad);
        }
    }

    private void FadeAndLoad()
    {
        _sceneFader.FadeAndLoadScene(_sceneName);
    }
   
}
