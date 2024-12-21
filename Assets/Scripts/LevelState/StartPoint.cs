using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public AudioClip startAudioClip;

    private bool isAlreadyExited = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAlreadyExited)
        {
            if (collision.CompareTag("Player"))
            {
                isAlreadyExited = true;
                animator.SetTrigger("move");
                SoundFXManager.instance.PlaySoundFXClip(startAudioClip, transform.position);
            }
        }
    }
}
