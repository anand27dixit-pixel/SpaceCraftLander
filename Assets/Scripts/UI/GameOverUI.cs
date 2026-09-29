using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    // Start is called before the first frame update
    void Awake()
    {
        mainMenuButton.onClick.AddListener(delegate
        {
            SceneLoader.LoadScene(GAME_SCENE.MainMenu.ToString());
        });

    }

    void Start()
    {
        SetFinalScore();
        mainMenuButton.Select();
    }

    private void SetFinalScore()
    {
        finalScoreText.text = string.Concat("FINAL SCORE :" + GameManager.Instance.GetTotatlScore());
    }
}
