using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausePopupUI : MonoBehaviour
{
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private Button musicVolumneButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private TextMeshProUGUI soundVolumnTextMesh;
    [SerializeField] private TextMeshProUGUI musicVolumnTextMesh;
    // Start is called before the first frame update

    void Awake()
    {
         soundVolumeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangedSoundVolumne();
            soundVolumnTextMesh.text = string.Concat("SOUND " + SoundManager.Instance.GetSoundVolumn());
        });

        musicVolumneButton.onClick.AddListener(() =>
       {
           MusicManager.Instance.ChangedMusicVolumn();
           musicVolumnTextMesh.text = string.Concat("MUSIC " + MusicManager.Instance.GetMusicVolumn());
       });

        resumeButton.onClick.AddListener(delegate
        {
            GameManager.Instance.OnGameUnPause();
            ShowHidePopup(false);
        });

        mainMenuButton.onClick.AddListener(delegate
        {
            SceneLoader.LoadScene(GAME_SCENE.MainMenu.ToString());
        });
    }

    void Start()
    {
        GameManager.Instance.evGamePause += OnGamePause;
        GameManager.Instance.evGameResume += OnGameResume;

        soundVolumnTextMesh.text = string.Concat("SOUND " + SoundManager.Instance.GetSoundVolumn());
        musicVolumnTextMesh.text = string.Concat("MUSIC " + MusicManager.Instance.GetMusicVolumn());

        ShowHidePopup(false);
    }

    private void OnGameResume(object sender, EventArgs e)
    {
        ShowHidePopup(false);
    }

    private void OnGamePause(object sender, EventArgs e)
    {
        ShowHidePopup(true);
    }

    private void ShowHidePopup(bool stat)
    {
        gameObject.SetActive(stat);
        if (stat)
            resumeButton.Select();
    }
}
