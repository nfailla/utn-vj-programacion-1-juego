using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    private float _minXPosition;

    private void Start()
    {
        _minXPosition = transform.position.x;
    }

    private void LateUpdate()
    {
        if (!followTarget) return;

        // This way the camera never goes back
        float targetXPosition = Mathf.Max(_minXPosition, followTarget.transform.position.x);
        float targetYPosition = followTarget.transform.position.y;

        _minXPosition = targetXPosition;

        transform.position = new Vector3(
            targetXPosition,
            targetYPosition,
            transform.position.z // don't move on Z
        );
    }
}
