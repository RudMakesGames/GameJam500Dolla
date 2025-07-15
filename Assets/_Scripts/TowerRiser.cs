using UnityEngine;
using UnityEngine.Events;

public class TowerRiser : MonoBehaviour
{
    TowerPuzzleLogic towerScript;
    public UnityEvent RiseEvent;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RiseEvent?.Invoke();
        }
    }



}
