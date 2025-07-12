using UnityEngine;

public class Attack : MonoBehaviour
{
    public int DamageAmount = 25;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            LanternController controller = collision.gameObject.GetComponentInChildren<LanternController>();
            controller.DamageLight(25);


        }
    }
}
