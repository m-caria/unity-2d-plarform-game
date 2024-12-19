using System.Collections;
using UnityEngine;

public class FireTrapBlock : MonoBehaviour
{
    public GameObject fireTrap;
    public int trapQuantity = 1;
    public float fireOffDuration = 3.0F;
    public float fireOnDuration = 2.0F;
    public Vector2 soundMinAndMaxDistance = new(1.0F, 10.0F);
    public AudioClip fireAudioClip;

    private static FireTrap[] fireTraps;
    private BoxCollider2D boxCollider;
    private bool isFireOn = false;
    private bool isFired = false;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        fireTraps = new FireTrap[trapQuantity];
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.TryGetComponent<FireTrap>(out FireTrap trap))
                fireTraps[i] = trap;
        }

        StartCoroutine(FireCycle());
    }

    private IEnumerator FireCycle()
    {
        while (true)
        {
            UpdateTrapsAnimation(true);
            Vector2 soundPosition = new(boxCollider.offset.x, 0.0F);
            SoundFXManager.instance.PlaySoundFXClip(fireAudioClip, soundPosition, soundMinAndMaxDistance, destroyAfter: fireOnDuration, parent: transform);
            yield return new WaitForSeconds(fireOnDuration);

            UpdateTrapsAnimation(false);
            yield return new WaitForSeconds(fireOffDuration);
        }
    }

    private void UpdateTrapsAnimation(bool animate)
    {
        isFireOn = animate;
        foreach (FireTrap trap in fireTraps)
            trap.SetFireStat(animate);
    }

    public void Generate()
    {
        Vector2 startPosition = transform.position;
        boxCollider = GetComponent<BoxCollider2D>();
        fireTraps = new FireTrap[trapQuantity];

        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        Vector2 spriteSize = fireTrap.GetComponent<SpriteRenderer>().size;
        float fireTrapSizeX = spriteSize.x * fireTrap.transform.localScale.x;

        for (int i = 0; i < trapQuantity; i++)
        {
            Vector2 newPosition = new(transform.position.x + fireTrapSizeX * (float)(i + 1), transform.position.y);
            FireTrap trap = Instantiate(fireTrap, newPosition, Quaternion.identity, transform).GetComponent<FireTrap>();
            fireTraps[i] = trap;
        }

        boxCollider.size = new Vector2(fireTrapSizeX * trapQuantity, (spriteSize.y * fireTrap.transform.localScale.y) / 2);
        boxCollider.offset = new Vector2((boxCollider.size.x / 2) + (fireTrapSizeX / 2), boxCollider.size.y / 2);

        transform.position = startPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FirePlayer(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FirePlayer(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isFired = false;
        }
    }

    private void FirePlayer(Collider2D collision)
    {
        if (isFireOn && !isFired)
        {
            isFired = true;
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.OnHitTrap(1);
        }
    }
}
