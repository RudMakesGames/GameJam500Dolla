using UnityEngine;

public class WiltingChase : MonoBehaviour
{
    public float ChaseSpeed = 5;


    private void Update()
    {
        transform.position += Vector3.right * ChaseSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            LanternController lanternController = collision.gameObject.GetComponentInChildren<LanternController>();
            lanternController.LightDeductionAmount = lanternController.LightDeductionAmount + 0.75f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LanternController lanternController = collision.gameObject.GetComponentInChildren<LanternController>();
            lanternController.LightDeductionAmount =  0.25f;
        }
    }
}
