using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public float player1Score;
    public TextMeshProUGUI player1ScoreText;
    static public float player2Score;
    public TextMeshProUGUI player2ScoreText;

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

            player1ScoreText.text = player1Score.ToString();
            player2ScoreText.text = player2Score.ToString();
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