using System.Collections;
using UnityEngine;

public class GoombaStomp : MonoBehaviour
{
    public float BounceAmount;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
          StartCoroutine(KillEnemy(collision.gameObject));
        }
    }
    IEnumerator KillEnemy(GameObject target)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, BounceAmount);
        target.GetComponent<Collider2D>().enabled = false;
        Animator anim = target.GetComponent<Animator>();
        anim?.SetTrigger("Dead");
        yield return new WaitForSeconds(0.45f);
        Destroy(target.gameObject);
        
    }
}
