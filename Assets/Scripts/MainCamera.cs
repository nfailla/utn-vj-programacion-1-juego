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

        float targetXPosition = Mathf.Max(_minXPosition, followTarget.transform.position.x);
        
        // so the camera never goes back
        _minXPosition = targetXPosition;

        transform.position = new Vector3(
            targetXPosition,
            transform.position.y, // don't move on Y
            transform.position.z // don't move on Z
        );
    }
}
