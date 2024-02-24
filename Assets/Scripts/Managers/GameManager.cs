using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static public float player1Score;
    public TextMeshProUGUI player1ScoreText;
    static public float player2Score;
    public TextMeshProUGUI player2ScoreText;

    public GameObject gameOverUI;

    private GameObject[] players;
    private bool isGameOver = false;

    void Start()
    {
        Time.timeScale = 1;

        player1Score = 0;
        player2Score = 0;

        gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
        {
            List<GameObject> playerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
            GameObject mergedPlayer = GameObject.FindGameObjectWithTag("MergedPlayer");

            if (mergedPlayer != null && mergedPlayer.activeSelf)
            {
                playerList.Add(mergedPlayer);
            }

            players = playerList.ToArray();

            if (players.Length == 0)
            {
                StartCoroutine(LerpTimeScaleToZero(2f));
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

        gameOverUI.SetActive(true);
    }
}
