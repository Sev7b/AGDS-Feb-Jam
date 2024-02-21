using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject[] players;
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length == 0)
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
