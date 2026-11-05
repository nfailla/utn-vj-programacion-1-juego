using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private float normalVolume = .5f;
    [SerializeField] private float pausedVolume = .1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Play(levelMusic);
        PauseManager.Instance.OnPause += HandlePause;
    }

    private void OnDestroy()
    {
        PauseManager.Instance.OnPause -= HandlePause;
    }


    private void HandlePause(bool isPaused)
    {
        _audioSource.volume = isPaused ? pausedVolume : normalVolume;
    }

    private void Play(AudioClip clip)
    {
        if (clip == null) return;

        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
