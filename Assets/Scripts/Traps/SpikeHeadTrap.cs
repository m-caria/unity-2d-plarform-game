using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHeadTrap : MonoBehaviour
{
    public enum Direction
    {
        BOTTOM = 0,
        TOP = 1,
        LEFT = 2,
        RIGHT = 3,
    }

    public struct DirectionData
    {
        public Vector2 DetectDirection;
        public Vector2 DetectionArea;
    }

    [Header("General Settings")]
    public Direction direction;

    [Header("Detection Settings")]
    public float detectionRange = 5.0F;
    public float detectionWidth = 2.0F;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    [Header("Movement Settings")]
    public float speedIncrement = 1.0F;
    public float maxSpeed = 5.0F;
    public float attackDelay = 2.0F;
    public float returnSpeed = 3.0F;

    [Header("Audio Settings")]
    public AudioClip crashAudioClip;
    public AudioClip playerCheckAudioClip;
    public Vector2 soundMinAndMaxDistance = new(1.0F, 10.0F);

    private Dictionary<Direction, DirectionData> vectorDirection;

    private Animator animator;
    private Rigidbody2D rb;

    private bool isMoving = false;
    private bool isTriggered = false;
    private bool isInGround = false;
    private bool isAtStartPosition = true;
    private float currentSpeed = 0.0F;
    private Vector2 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetInteger("direction", (int)direction);
        startPosition = transform.position;
        vectorDirection = new()
        {
            { Direction.TOP, new DirectionData { DetectDirection = Vector2.up, DetectionArea = new Vector2(detectionWidth, detectionRange) } },
            { Direction.LEFT, new DirectionData { DetectDirection = Vector2.left, DetectionArea = new Vector2(detectionRange, detectionWidth) } },
            { Direction.RIGHT, new DirectionData { DetectDirection = Vector2.right, DetectionArea = new Vector2(detectionRange, detectionWidth) } },
            { Direction.BOTTOM, new DirectionData { DetectDirection = Vector2.down, DetectionArea = new Vector2(detectionWidth, detectionRange) } },
        };
    }

    private void Update()
    {
        if (!isTriggered && isAtStartPosition)
        {
            DetectPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        
        currentSpeed = Mathf.Min(currentSpeed + speedIncrement * Time.fixedDeltaTime, maxSpeed);
        rb.linearVelocity = vectorDirection[direction].DetectDirection * currentSpeed;

        if (Physics2D.Raycast(transform.position, vectorDirection[direction].DetectDirection, 0.5F, groundLayer))
        {
            if (!isInGround)
            {
                StopMovement();
                animator.SetTrigger("animate");
                SoundFXManager.instance.PlaySoundFXClip(crashAudioClip, transform.position, soundMinAndMaxDistance);
                Invoke(nameof(MoveToStartPosition), 2.0F);
            }

            isInGround = true;
        }

        if (!isAtStartPosition && isMoving && isInGround)
            MoveToStartPosition();
    }

    private void DetectPlayer()
    {
        Vector2 boxCenter = (Vector2)transform.position + vectorDirection[direction].DetectDirection * detectionRange / 2;
        Vector2 boxSize = vectorDirection[direction].DetectionArea;

        if (Physics2D.OverlapBox(boxCenter, boxSize, 0, playerLayer))
        {
            if (isTriggered) return;

            SoundFXManager.instance.PlaySoundFXClip(playerCheckAudioClip, transform.position, soundMinAndMaxDistance);
            isTriggered = true;
            animator.SetTrigger("blink");
            Invoke(nameof(StartMovement), attackDelay);
        }
    }

    private void StartMovement()
    {
        isMoving = true;
        isAtStartPosition = false;
    }

    private void StopMovement()
    {
        isMoving = false;
        currentSpeed = 0.0F;
        rb.linearVelocity = Vector2.zero;

        isTriggered = false;
    }

    private void MoveToStartPosition()
    {
        isMoving = true;

        rb.linearVelocity = returnSpeed * Time.fixedDeltaTime * -vectorDirection[direction].DetectDirection;

        if (Vector2.Distance(startPosition, transform.position) < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
            isInGround = false;
            isAtStartPosition = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.OnHitTrap(1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 detectionArea = direction == Direction.LEFT || direction == Direction.RIGHT
            ? new Vector3(detectionRange, detectionWidth, 0)
            : new Vector3(detectionWidth, detectionRange, 0);

        Vector3 detectionOffset = direction switch
        {
            Direction.LEFT => Vector3.left * detectionRange / 2,
            Direction.RIGHT => Vector3.right * detectionRange / 2,
            Direction.TOP => Vector3.up * detectionRange / 2,
            Direction.BOTTOM => Vector3.down * detectionRange / 2,
            _ => Vector3.zero
        };

        Gizmos.DrawWireCube(transform.position + detectionOffset, detectionArea);
    }
}
