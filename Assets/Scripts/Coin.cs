using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip sound;
    private bool hasBeenTriggered;
    private int value = 100;

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "Player" && !hasBeenTriggered) {
            hasBeenTriggered = true;
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddCoin(value);
            Destroy(gameObject);
        }
    }


}
