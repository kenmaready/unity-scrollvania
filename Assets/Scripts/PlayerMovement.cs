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
}

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;
    PlayerClass player = new PlayerClass();
    Vector2 moveInput;
    bool isAlive = true;
    float runSpeed = 4.0f;
    float jumpSpeed = 7.0f;
    float climbSpeed = 2.0f;

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
            player.rigidbody.velocity += new Vector2(0f, jumpSpeed);
            Debug.Log("Jumping!");
        }
    }

    private void OnFire(InputValue value) {
        if (!isAlive) { return; }
        player.animator.SetTrigger("Fire");
        Instantiate(arrow, bow.position, transform.rotation);
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
        }
    }

    private void Die() {
        if (isAlive && player.bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            isAlive = false;
            player.animator.SetTrigger("Dying");
            CinemachineShake.Instance.ShakeCamera(15.0f, 0.1f);
            player.rigidbody.velocity += new Vector2(0f, jumpSpeed);
            player.rigidbody.velocity += new Vector2(0f, 0f);
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
}
