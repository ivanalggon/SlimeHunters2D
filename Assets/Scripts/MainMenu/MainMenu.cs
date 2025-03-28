using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartPlaying()
    {
        SceneManager.LoadScene("Game");
    }
    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("PrincipalMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
