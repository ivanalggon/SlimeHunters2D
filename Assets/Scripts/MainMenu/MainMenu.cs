using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartPlaying()
    {
        // Load the game scene
        SceneManager.LoadScene("Game");
    }
    public void ShowCredits()
    {
        // Load the credits scene
        SceneManager.LoadScene("Credits");
    }
    public void ReturnMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("PrincipalMenu");
    }
    public void ExitGame()
    {
        // Exit the game
        Application.Quit();
    }
}
