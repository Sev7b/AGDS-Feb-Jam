using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }
}
