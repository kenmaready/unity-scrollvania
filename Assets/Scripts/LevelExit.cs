using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] AudioClip sound;
    float levelLoadDelay = 1f;
    private bool portalTriggered = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !portalTriggered) {
            portalTriggered = true;
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel() {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }

        Debug.Log("No. of ScenePersists: " + FindObjectsOfType<ScenePersist>().Length);

        FindObjectOfType<ScenePersist>().Reset();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
