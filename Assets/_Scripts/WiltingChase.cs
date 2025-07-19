using Unity.VisualScripting;
using UnityEngine;

public class WiltingChase : MonoBehaviour
{
    public float ChaseSpeed = 5;
    public Sprite WitheringSprite;
    public Color32 DarkSkyColor,CloudColor;
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
        if (collision.gameObject.CompareTag("Sky"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = DarkSkyColor;
        }
        if (collision.gameObject.CompareTag("Cloud"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = CloudColor;
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
