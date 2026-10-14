using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform FollowTarget { get; set; }

    private float _minXPosition;

    private void Awake()
    {
        FollowTarget = GameObject.Find("Player").transform; // TODO: it's too brittle
    }

    private void Start()
    {
        _minXPosition = transform.position.x;
    }

    private void LateUpdate()
    {
        if (!FollowTarget) return;

        float targetXPosition = Mathf.Max(_minXPosition, FollowTarget.transform.position.x);
        
        // so the camera never goes back
        _minXPosition = targetXPosition;

        transform.position = new Vector3(
            targetXPosition,
            transform.position.y, // don't move on Y
            transform.position.z // don't move on Z
        );
    }

    public void RestartToPosition(Vector3 position)
    {
        // update the current minXPosition accordingly
        _minXPosition = position.x;

        transform.position = new Vector3(
            position.x,
            position.y,
            transform.position.z // never move on Z!
        );
    }
}
