using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public AudioClip checkpointAudioClip;

    private bool isAlreadyExited = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlreadyExited)
        {
            if (collision.CompareTag("Player"))
            {
                isAlreadyExited = true;
                animator.SetTrigger("animate");
                SoundFXManager.instance.PlaySoundFXClip(checkpointAudioClip, transform.position);
            }
        }
    }
}
