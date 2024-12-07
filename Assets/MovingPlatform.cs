using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool requireButtonToMove = false;
    public Lever lever;

    public Transform startWaypoint;
    public Transform endWaypoint;

    public float platformSpeed = 2f;
    private bool isMoving = false;
    private bool isMovingToEnd = false;

    private void Start()
    {
        if(requireButtonToMove)
        {
            lever.onLeverOn.AddListener(MovePlatform);
            lever.onLeverOff.AddListener(StopPlatform);
        }
        else{
            MovePlatform();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move the platform
            if (isMovingToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endWaypoint.position, platformSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, endWaypoint.position) < 0.1f)
                {
                    isMovingToEnd = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startWaypoint.position, platformSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startWaypoint.position) < 0.1f)
                {
                    isMovingToEnd = true;
                }
            }
        }
    }

    private void MovePlatform()
    {
        isMoving = true;
    }

    private void StopPlatform()
    {
        isMoving = false;
    }
}