using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake() {
        int scenePersists = FindObjectsOfType<ScenePersist>().Length;

        if (scenePersists > 1) {
            Debug.Log("Destroying ScenePersist....");
            Destroy (gameObject);
        } else {
            Debug.Log("Keeping this ScenePersist...");
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Reset() {
        Destroy(gameObject);
    }
}
