using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public int livesToRemove = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.OnHitTrap(livesToRemove);
        }
    }
}
