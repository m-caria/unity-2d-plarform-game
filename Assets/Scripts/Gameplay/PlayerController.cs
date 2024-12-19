using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Controls")]
    public float movementSpeed = 5.0F;
    public float jumpForce = 5.0F;
    public float wallSlideFriction = 0.2F;
    public Vector2 wallJumpDirection = new(3, 6);
    public bool canJumpOnWall = true;

    [Header("Ground Check")]
    public float groundCastDistance;
    public float wallCastDistance;
    public LayerMask groundLayer;

    [Header("Stats")]
    public PlayerStats playerStats;

    [Header("Audio")]
    public AudioClip jumpAudioClip;
    public AudioClip hitAudioClip;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private PlayerControls controls;
    private Animator animator;
    private PlayerAnimations playerAnimations;
    private PlayerControllerStats playerControllerStats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

        playerAnimations = new AnimationsManager(animator).GetPlayerAnimations();
        playerControllerStats.IsFacingRight = true;

        controls = new PlayerControls();
        controls.Player_Gameplay.Movement.performed += ctx => playerControllerStats.Movement = ctx.ReadValue<float>();
        controls.Player_Gameplay.Movement.canceled += ctx => playerControllerStats.Movement = 0;
        controls.Player_Gameplay.Jump.performed += ctx => Jump();
    }

    private void Start()
    {
        GameManager.GetGUI().PrepareGUI(playerStats);
    }

    private void Update()
    {
        if (GameManager.GetGUI().IsTimerEnd())
            GameManager.GetGUI().GameOver();

        if (!GameManager.IsGamePaused)
        {
            CheckCasts();
            FlipController();

            if (playerControllerStats.IsGrounded)
            {
                playerControllerStats.CanDoubleJump = true;
                playerControllerStats.CanMove = true;
            }

            playerControllerStats.CanSlideOnWall = !playerControllerStats.IsGrounded && rb.linearVelocityY < 0;
            playerAnimations.UpdateAnimations(
                rb.linearVelocityY,
                playerControllerStats.IsGrounded,
                playerControllerStats.IsWallSliding
            );
        }
    }

    private void FlipController()
    {
        if (playerControllerStats.IsGrounded && playerControllerStats.IsOnWall)
        {
            if (playerControllerStats.IsFacingRight && playerControllerStats.Movement < 0) Flip();
            else if (!playerControllerStats.IsFacingRight && playerControllerStats.Movement > 0) Flip();
        }

        if (playerControllerStats.Movement > 0 && !playerControllerStats.IsFacingRight) Flip();
        else if (playerControllerStats.Movement < 0 && playerControllerStats.IsFacingRight) Flip();
    }

    private void FixedUpdate()
    {
        if (playerControllerStats.IsOnWall && playerControllerStats.CanSlideOnWall)
        {
            if (playerControllerStats.Movement == 0 || Mathf.Sign(playerControllerStats.Movement) != Mathf.Sign(GetWallDirection()))
            {
                playerControllerStats.IsWallSliding = false;
                return;
            }

            playerControllerStats.IsWallSliding = true;
            rb.linearVelocityY *= wallSlideFriction;
        } 
        else if (!playerControllerStats.IsOnWall)
        {
            playerControllerStats.IsWallSliding = false;
            Move();
        }
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private float GetWallDirection() => playerControllerStats.IsFacingRight ? 1 : -1; 

    public void OnHitTrap(int damage)
    {
        SoundFXManager.instance.PlaySoundFXClip(hitAudioClip, transform.position);
        playerAnimations.Hit();
        playerStats.Lives -= damage;
        GameManager.GetGUI().RemoveLive();
    }

    public void OnCollectFruit(int quanity)
    {
        playerStats.Fruits += quanity;
        GameManager.GetGUI().UpdateFruit(playerStats.Fruits);
    }

    private void Move()
    {
        if (playerControllerStats.CanMove && !GameManager.IsGamePaused)
        {
            rb.linearVelocity = new Vector2(playerControllerStats.Movement * movementSpeed, rb.linearVelocityY);
            playerAnimations.Run(playerControllerStats.Movement != 0);
        }
    }

    private void Jump()
    {
        if (!GameManager.IsGamePaused) 
        {
            if (playerControllerStats.IsWallSliding && canJumpOnWall)
            {
                SoundFXManager.instance.PlaySoundFXClip(jumpAudioClip, transform.position);
                WallJump();
            }
            else if (playerControllerStats.IsGrounded)
            {
                SoundFXManager.instance.PlaySoundFXClip(jumpAudioClip, transform.position);
                PerformJump();
            }
            else if (playerControllerStats.CanDoubleJump)
            {
                SoundFXManager.instance.PlaySoundFXClip(jumpAudioClip, transform.position, pitch: 1.5F);
                playerControllerStats.CanMove = true;
                playerControllerStats.CanDoubleJump = false;
                PerformJump();
                playerAnimations.DoubleJump();
            }
        }
    }

    private void WallJump()
    {
        if (Mathf.Sign(playerControllerStats.Movement) == Mathf.Sign(GetWallDirection()))
        {
            playerControllerStats.CanMove = false;
            Vector2 direction = new(wallJumpDirection.x * -playerControllerStats.Movement, wallJumpDirection.y);
            rb.AddForce(direction, ForceMode2D.Impulse);
            Flip();
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
    }

    private void Flip()
    {
        if (!playerControllerStats.IsOnWall)
        {
            playerControllerStats.IsFacingRight = !playerControllerStats.IsFacingRight;
            transform.Rotate(0, 180, 0);
        }
        
    }

    private void CheckCasts()
    {
        playerControllerStats.IsGrounded = Physics2D.CapsuleCast(
            capsuleCollider.bounds.center,
            capsuleCollider.bounds.size,
            CapsuleDirection2D.Vertical, 0,
            Vector2.down,
            groundCastDistance,
            groundLayer
        );

        playerControllerStats.IsOnWall = Physics2D.CapsuleCast(
            capsuleCollider.bounds.center,
            capsuleCollider.bounds.size,
            CapsuleDirection2D.Horizontal, 0,
            Vector2.left,
            wallCastDistance,
            groundLayer
         ) && rb.linearVelocityY != 0;
    }
}
