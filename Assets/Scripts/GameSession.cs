using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    int lives = 3;
    int coins = 0;

    void Awake() {
        int numSessions = FindObjectsOfType<GameSession>().Length;
        if (numSessions > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath() {
        if (lives> 1) {
            lives--;
            ResetLevel();
        } else {
            ResetGameSession();
        }
    }

    public void AddCoin() {
        coins++;
        Debug.Log("Another coin! Player now has: " + coins + " coins.");
    }

    private void ResetLevel() {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void ResetGameSession() {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
