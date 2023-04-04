using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI livesDisplay;
    [SerializeField] TextMeshProUGUI coinDisplay;
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

    private void Start() {
        UpdateHUD();
    }

    public void ProcessPlayerDeath() {
        if (lives> 1) {
            Debug.Log("Decrementing lives...");
            lives--;
            Debug.Log("Lives remaining: " + lives);
            UpdateHUD();
            ResetLevel();
        } else {
            ResetGameSession();
        }
    }

    public void AddCoin(int value) {
        coins += value;
        UpdateHUD();
        Debug.Log("Another coin! Player now has: " + coins + " coins.");
    }

    private void UpdateHUD() {
        Debug.Log("Updating HUD: Lives: " + lives.ToString() + " | Coins: " + coins.ToString());
        livesDisplay.text = lives.ToString().PadLeft(2, '0');
        coinDisplay.text = coins.ToString().PadLeft(5, '0');
    }

    private void ResetLevel() {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void ResetGameSession() {
        FindObjectOfType<ScenePersist>().Reset();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
