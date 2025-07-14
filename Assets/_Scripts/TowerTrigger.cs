using UnityEngine;
using UnityEngine.Events;

public class TowerTrigger : MonoBehaviour
{
    public UnityEvent Event;
    public UnityEvent Exit;
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
}
