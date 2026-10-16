using UnityEngine;

[DefaultExecutionOrder(100)] // Must execute after MainCamera script!
public class ParallaxLayer : MonoBehaviour
{
    /**
     * How close to the camera this layer will move.
     * 1f = static (won't move at all)
     * 0f = will stick to the camera movement
     */
    [SerializeField] private float parallaxFactor = 1f;
    [SerializeField] private Transform cameraTransform;

    private Vector3 _previousCameraPosition;

    private void Start()
    {
        _previousCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 cameraDelta = cameraTransform.position - _previousCameraPosition;

        // After knowing the camera movement delta,
        // move a certain amount (weighted by the parallaxFactor) towards the camera.
        // We only move in X
        transform.position += new Vector3(
            cameraDelta.x * parallaxFactor,
            0f,
            0f
        );

        _previousCameraPosition = cameraTransform.position;
    }
}
