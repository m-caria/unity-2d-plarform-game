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
    public GameObject collectedAnimation;
    public AudioClip collectedAudioClip;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("item", (int)type);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerController>(out PlayerController playerController))
                playerController.OnCollectFruit(1);

            SoundFXManager.instance.PlaySoundFXClip(collectedAudioClip, transform.position);
            Instantiate(collectedAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
