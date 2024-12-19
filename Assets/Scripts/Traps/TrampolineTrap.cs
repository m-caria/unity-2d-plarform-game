using UnityEngine;

public class TrampolineTrap : MonoBehaviour
{
    public float jumpForce = 50.0F;
    public AudioClip launchAudioClip;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("jump");
            SoundFXManager.instance.PlaySoundFXClip(launchAudioClip, transform.position, new Vector2(0.3F, 15.0F));

            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
