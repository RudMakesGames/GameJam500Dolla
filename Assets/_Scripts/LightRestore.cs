using UnityEngine;
using UnityEngine.Events;

public class LightRestore : MonoBehaviour
{
    public UnityEvent Event;
    public float RestoreAmount = 70f;
    public AudioClip LanternRestore;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Event?.Invoke();
            LanternController lanternController = collision.gameObject.GetComponentInChildren<LanternController>();
            lanternController.RestoreLight(RestoreAmount);
            AudioManager.instance.PlaySoundFXClip(LanternRestore,transform,1,Random.Range(0.9f,1.1f));
            Destroy(gameObject);
        }
    }
}
