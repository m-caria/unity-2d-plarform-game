using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    public enum ItemType
    {
        APPLE = 0,
        BANANA = 1,
        CHERRY = 2,
        MELON = 3,
        KIWI = 4,
        STRAWBERRY = 5,
        PINEAPPLE = 6,
        ORANGE = 7,
    }

    public ItemType type = ItemType.APPLE;
    public int liveRestoreQuantity = 0;
    public GameObject collectedAnimation;
    public AudioClip collectedAudioClip;
    public LayerMask groundLayerMask;

    private Animator animator;
    private CircleCollider2D circleCollider;

    private bool isMoving = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("item", (int)type);
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (IsInGround() && isMoving)
        {
            StopMovements();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerController>(out PlayerController playerController))
            {
                if (liveRestoreQuantity > 0)
                    playerController.OnRestoreLive(liveRestoreQuantity);
                else playerController.OnCollectFruit(1);
            }
                
            SoundFXManager.instance.PlaySoundFXClip(collectedAudioClip, transform.position);
            Instantiate(collectedAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void StopMovements()
    {
        isMoving = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0F;
        rb.linearVelocity = Vector2.zero;
    }

    private bool IsInGround()
    {
        return Physics2D.CircleCast(
            circleCollider.bounds.center,
            circleCollider.radius,
            Vector2.down, 0.0F, groundLayerMask
        );
    }
}
