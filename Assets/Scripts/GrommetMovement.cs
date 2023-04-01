using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrommetMovement : MonoBehaviour
{

    Rigidbody2D rb;
    float moveSpeed = 1f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag != "Player") {
            moveSpeed *= -1f;
            FlipSprite();
        }
    }

    private void FlipSprite() {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
