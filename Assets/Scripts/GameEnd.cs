using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    public string endGameScene = "EndGameScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("End Game Triggered");
        SceneManager.LoadScene(endGameScene);
    }
}
