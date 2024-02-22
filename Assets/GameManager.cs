using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject[] players;
    private bool isGameOver = false;

    void Update()
    {
        if (!isGameOver)
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length == 0)
            {
                StartCoroutine(LerpTimeScaleToZero(1f));
                isGameOver = true; 
            }
        }
    }

    IEnumerator LerpTimeScaleToZero(float duration)
    {
        float start = Time.timeScale;
        float time = 0;

        while (time < duration)
        {
            Time.timeScale = Mathf.Lerp(start, 0, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        SceneManager.LoadScene("Game Over"); 
    }
}
