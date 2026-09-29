using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_SCENE
{
    MainMenu,
    Game,
    GameOver
}

public static class SceneLoader
{
    public static void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
