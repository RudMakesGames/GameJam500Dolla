using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform waypoint2;
    [SerializeField, Range(0f, 10f)] private float speed = 2f;

    private GameObject Player;
    private LanternController lanternController;
    [SerializeField]
    private Transform currentTarget;
    private bool isMoving = false;
    public float Threshold;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        lanternController = Player.GetComponentInChildren<LanternController>();
        currentTarget = waypoint1;
        
    }

    private void Update()
    {
        if (isMoving && currentTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.05f)
            {
                isMoving = false;
            }
        }
    }

    /// <summary>
    /// Call this to move the platform to the other waypoint.
    /// </summary>
    public void TriggerPlatformMovement()
    {
        Debug.Log("Triggered!");
        if (isMoving) return; // Prevent mid-move trigger

        Debug.Log("Triggered!");

        if (currentTarget == waypoint1)
            currentTarget = waypoint2;
        else
            currentTarget = waypoint1;

        isMoving = true;
    }
}

