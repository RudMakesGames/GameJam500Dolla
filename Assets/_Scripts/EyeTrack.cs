using UnityEngine;

public class EyeTrack : MonoBehaviour
{
    Transform Player;

    [SerializeField] float factor;

    Vector2 ogPos;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        ogPos = transform.localPosition;
    }

    private void Update()
    {
        Vector2 dirn = (Player.transform.position - transform.position).normalized;

        Debug.DrawRay(transform.position, dirn*(Player.transform.position - transform.position), Color.red);

        /*Vector2 distanceAllowed = new Vector2(dirn.x, dirn.y)*factor;*/

        transform.localPosition = ogPos + dirn*factor;
    }
}
