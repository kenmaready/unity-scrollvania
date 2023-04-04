using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrommetMovement : MonoBehaviour
{

    Rigidbody2D rb;
    float moveSpeed = 1f;
    [SerializeField] int hitPoints = 1;

    protected void Awake() {
        rb = GetComponent<Rigidbody2D>();
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

    public void TakeHit() {
        hitPoints --;
        Debug.Log("I got shot! " + hitPoints.ToString() + " hit points left.");
        if (hitPoints < 1) { 
            Destroy(gameObject); 
        } else {
            Color currentColor = GetComponent<SpriteRenderer>().color;
            currentColor.a = currentColor.a - 0.1f;
            GetComponent<SpriteRenderer>().color = currentColor;
        }
    }
}
