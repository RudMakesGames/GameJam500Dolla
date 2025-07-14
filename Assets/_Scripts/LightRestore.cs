using UnityEngine;

public class LightRestore : MonoBehaviour
{
    public float RestoreAmount = 70f;
    public AudioClip LanternRestore;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            LanternController lanternController = collision.gameObject.GetComponentInChildren<LanternController>();
            lanternController.RestoreLight(RestoreAmount);
            AudioManager.instance.PlaySoundFXClip(LanternRestore,transform,1,Random.Range(0.9f,1.1f));
            Destroy(gameObject);
        }
    }
}
