using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    public int DamageAmount = 50;

    public bool canRespawn = false;

    public Transform RespawnPoint;
    public UnityEvent Event;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Transform playerTransform = collision.gameObject.transform;
            if(canRespawn)
            {
                Event?.Invoke();
                playerTransform.position = RespawnPoint.position;
            }
            else
            {
                LanternController controller = collision.gameObject.GetComponentInChildren<LanternController>();
                controller.TakeDamage(DamageAmount);
            }
        }
    }
}
