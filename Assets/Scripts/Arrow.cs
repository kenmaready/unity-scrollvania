using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    Rigidbody2D rb;
    PlayerMovement player;
    float velocity = 10f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        velocity *= player.transform.localScale.x;
    }

    void Update()
    {
        rb.velocity = new Vector2(velocity, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            other.GetComponent<GrommetMovement>().TakeHit();
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") { return; }

        if (other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<GrommetMovement>().TakeHit();
        }
        Destroy(gameObject);
    }
}
