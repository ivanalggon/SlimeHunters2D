using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartPlaying()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
