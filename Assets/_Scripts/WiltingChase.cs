using UnityEngine;

public class WiltingChase : MonoBehaviour
{
    public float ChaseSpeed = 5;
    public Sprite WitheringSprite;

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
        if(collision.gameObject.CompareTag("Flower"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = WitheringSprite;
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
