using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private bool isAlreadyPressed = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlreadyPressed)
        {
            if (collision.CompareTag("Player"))
            {
                isAlreadyPressed = true;
                animator.SetTrigger("pressed");
            }
        }
    }
}
