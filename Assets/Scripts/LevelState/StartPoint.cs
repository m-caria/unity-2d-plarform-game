using UnityEngine;

public class StartPoint : MonoBehaviour
{
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
            }
        }
    }
}
