using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool hasBeenTriggered;
    [SerializeField] AudioClip sound;

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.tag == "Player" && !hasBeenTriggered) {
            hasBeenTriggered = true;
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddCoin();
            Destroy(gameObject);
        }
    }


}
