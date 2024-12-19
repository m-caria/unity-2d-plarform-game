using UnityEngine;

public class FireTrap : MonoBehaviour
{
    private Animator animator;
    private bool isFireOn = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetFireStat(bool isActive) => isFireOn = isActive;

    private void Update()
    {
        animator.SetBool("isFireOn", isFireOn);
    }
}
