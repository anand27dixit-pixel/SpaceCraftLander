using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        Time.timeScale=1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        playButton.onClick.AddListener(delegate
        {
            SceneLoader.LoadScene(GAME_SCENE.Game.ToString());
            // Reset static values
            GameManager.ResetGameStatics();
        });

        // Quit
       quitButton.onClick.AddListener(delegate
       {
           Application.Quit();
       });

       playButton.Select();
    }

}
