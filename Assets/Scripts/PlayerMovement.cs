using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClass {
    public Rigidbody2D rigidbody;
    public CapsuleCollider2D bodyCollider;
    public BoxCollider2D footCollider;
    public Animator animator;
    public float defaultGravity;
    public bool climbingThroughPlatform = false;
}

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    [SerializeField] AudioClip bowstringSFX;
    [SerializeField] AudioClip arrowSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip bounceSFX;
    PlayerClass player = new PlayerClass();
    Vector2 moveInput;
    bool isAlive = true;
    float runSpeed = 4.0f;
    float jumpSpeed = 10.0f;
    float climbSpeed = 2.0f;
    GameObject currentPlatform;

    void Awake() {
        player.rigidbody = GetComponent<Rigidbody2D>();
        player.bodyCollider = GetComponent<CapsuleCollider2D>();
        player.footCollider = GetComponent<BoxCollider2D>();
        player.animator = GetComponent<Animator>();
        player.defaultGravity = player.rigidbody.gravityScale;
    }

    void Update() {
        if (!isAlive) {
            return;
        }

        UpdatePlayerAnimatorState();
        Run();
        Climb();
        FlipSprite();
        Die();
    }

    private void OnMove(InputValue value) {
        if (!isAlive) {
            return;
        }

        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value) {
        if (!isAlive) {
            return;
        }
        if (value.isPressed && (IsTouchingGround() || IsTouchingBouncingObject())) {
            if (IsTouchingBouncingObject()) {
                AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
            } else {
                AudioSource.PlayClipAtPoint(jumpSFX, Camera.main.transform.position);
            }
            player.rigidbody.velocity += new Vector2(0f, jumpSpeed);
            Debug.Log("Jumping!");
        }
    }

    private void OnFire(InputValue value) {
        if (!isAlive) { return; }
        player.animator.SetTrigger("Fire");
        // AudioSource.PlayClipAtPoint(bowstringSFX, Camera.main.transform.position);
        AudioSource.PlayClipAtPoint(arrowSFX, Camera.main.transform.position);
        Vector3 rotation = transform.rotation.eulerAngles;
        if (transform.localScale.x == -1) {
            rotation = new Vector3(rotation.x, rotation.y, rotation.z + 180);
        }
        Instantiate(arrow, bow.position, Quaternion.Euler(rotation));
    }

    private void OnRoll(InputValue vlue) {
        if (!isAlive) { return; }

        player.animator.SetTrigger("Roll");
    }

    private void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, player.rigidbody.velocity.y);
        player.rigidbody.velocity = playerVelocity;
    }

    private void Climb() {
        if (IsTouchingLadder()) {
            player.rigidbody.gravityScale = 0f;
            if (IsMovingVertically()) {
                Vector2 playerVelocity = new Vector2(player.rigidbody.velocity.x, moveInput.y * climbSpeed);
                player.rigidbody.velocity = playerVelocity;
            } else {
                Vector2 playerVelocity = new Vector2(player.rigidbody.velocity.x, 0f);
                player.rigidbody.velocity = playerVelocity;
            }
        } else {
            player.rigidbody.gravityScale = player.defaultGravity;
            if (currentPlatform) {
                Physics2D.IgnoreCollision(currentPlatform.GetComponent<CompositeCollider2D>(), player.bodyCollider, false);
                Physics2D.IgnoreCollision(currentPlatform.GetComponent<CompositeCollider2D>(), player.footCollider, false);
            }
        }
    }



    private void Die() {
        if (isAlive && player.bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            isAlive = false;
            player.animator.SetTrigger("Dying");
            CinemachineShake.Instance.ShakeCamera(15.0f, 0.1f);
            player.rigidbody.velocity += new Vector2(0f, jumpSpeed);
            player.rigidbody.velocity += new Vector2(0f, 0f);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void FlipSprite() {
        if (IsRunning()) {
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x),1f);
        }
    }

    private void UpdatePlayerAnimatorState() {
        player.animator.SetBool("isRunning", IsRunning());
        player.animator.SetBool("isClimbing", IsClimbing());
    }

    private bool IsRunning() {
        float myEpsilon = 0.00005f;
        return Mathf.Abs(player.rigidbody.velocity.x) > myEpsilon;
    }

    private bool IsClimbing() {
        return IsTouchingLadder() && IsMovingVertically();
    }

    private bool IsMovingVertically() {
        // Debug.Log("In IsClimbing() - moveInput.y: " + moveInput.y);
        return (Mathf.Abs(moveInput.y) == 1) && IsTouchingLadder();
    }

    private bool IsTouchingGround() {
        bool isTouching = player.footCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        Debug.Log("isTouching Ground: " + isTouching);
        return isTouching;
    }

    private bool IsTouchingLadder() {
        bool isTouching = player.footCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        // Debug.Log("isTouching Ladder: " + isTouching);
        return isTouching;
    }

    private bool IsTouchingBouncingObject() {
        return player.bodyCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing"));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Platform" && IsClimbing()) {
            currentPlatform = other.gameObject;
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<CompositeCollider2D>(), player.bodyCollider, true);
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<CompositeCollider2D>(), player.footCollider, true);
        }
    }
}
