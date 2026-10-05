using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    private void LateUpdate()
    {
        transform.position = new Vector3(
            followTarget.transform.position.x,
            transform.position.y,
            transform.position.z
        );
    }
}
