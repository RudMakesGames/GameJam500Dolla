using UnityEngine;

public class LightRestore : MonoBehaviour
{
    public float RestoreAmount = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            LanternController lanternController = collision.gameObject.GetComponentInChildren<LanternController>();
            lanternController.RestoreLight(RestoreAmount);
            Destroy(gameObject);
        }
    }
}
