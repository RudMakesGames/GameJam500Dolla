using UnityEngine;
using UnityEngine.Events;

public class TowerTrigger : MonoBehaviour
{
    public UnityEvent Event;
    public UnityEvent Exit;

    public Transform DesiredDestination;
    public Transform PlayerPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Event?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Exit?.Invoke();
        }
    }

    public void Teleport()
    {
        Invoke("TeleportPlayer", 1);
    }
     void TeleportPlayer()
    {
        PlayerPos.position = DesiredDestination.position;
    }
}
