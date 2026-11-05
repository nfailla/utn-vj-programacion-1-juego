using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float delayAfterDestroy = 2f;
    
    [SerializeField] private string mainCameraTag = "MainCamera";

    private MainCamera _mainCamera;

    private void Awake()
    {
        GameObject camera = GameObject.FindGameObjectWithTag(mainCameraTag);
        camera.TryGetComponent(out _mainCamera);
    }

    private void Start()
    {
        _mainCamera.SomethingExploded();
        Destroy(gameObject, delayAfterDestroy);
    }
}
