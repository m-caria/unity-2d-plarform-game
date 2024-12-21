using UnityEngine;

public class BoxItem : MonoBehaviour
{
    [Header("General")]
    public GameObject boxExplosionParticles;
    public AudioClip boxExplosionSound;
    public GameObject collectableToSpawn;

    [Header("Settings")]
    public float bounceForce = 5.0F;
    public Vector2 collectableSpawnRange = new(1.0F, 5.0F);

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 contactNormal = contactPoint.normal;

            if (contactNormal.y < -0.5F)
                BreakFromUp(collision.collider);
            else if (contactNormal.y > 0.5F)
                BreakBox();
        }
    }

    private void BreakFromUp(Collider2D collider)
    {
        BreakBox();

        if (collider.TryGetComponent(out Rigidbody2D playerRb))
            playerRb.linearVelocityY = bounceForce;
    }

    private void BreakBox()
    {
        SpawnCollectable();
        SoundFXManager.instance.PlaySoundFXClip(boxExplosionSound, transform.position);
        animator.SetTrigger("animate");
    }

    private void SpawnCollectable()
    {
        int spawnQuantity = (int)Random.Range(collectableSpawnRange.x, collectableSpawnRange.y);

        for (int i = 0; i < spawnQuantity; i++) 
        { 
            GameObject collectable = Instantiate(collectableToSpawn, transform.position, Quaternion.identity);
            Rigidbody2D collectableRb = collectable.GetComponent<Rigidbody2D>();
            collectableRb.gravityScale = 1.0F;
            collectableRb.linearDamping = 2.0f;

            Vector2 randomForce = new(Random.Range(-5.0f, 5.0f), Random.Range(3.0f, 8.0f));
            collectableRb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }

    private void OnDestroy()
    {
        GameObject explosionParticle = Instantiate(boxExplosionParticles, transform.position, Quaternion.identity);
        if (explosionParticle.TryGetComponent(out ParticleSystem particleSystem))
            Destroy(explosionParticle, particleSystem.main.duration);
    }
}
