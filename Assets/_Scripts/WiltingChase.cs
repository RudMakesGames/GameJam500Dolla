using UnityEngine;

public class WiltingChase : MonoBehaviour
{
    public int ChaseSpeed = 5;


    private void Update()
    {
        transform.position += Vector3.right * ChaseSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
