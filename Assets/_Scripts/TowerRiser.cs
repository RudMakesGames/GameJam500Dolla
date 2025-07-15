using UnityEngine;
using UnityEngine.Events;

public class TowerRiser : MonoBehaviour
{
    TowerPuzzleLogic towerScript;
    public UnityEvent RiseEvent;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RiseEvent?.Invoke();
        }
    }



}
