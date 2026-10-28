using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private AudioSource _audioSource;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;

        _audioSource.pitch = pitch;

        _audioSource.PlayOneShot(clip);
    }

    public void Play(AudioClip[] clips, float pitch = 1f)
    {
        if (clips == null || clips.Length == 0) return;

        int randIndex = Random.Range(0, clips.Length);
        Play(clips[randIndex], pitch);
    }

    public void PlaySequentially(AudioClip[] clips, float pitch = 1f)
    {
        StartCoroutine(PlaySequenceCoroutine(clips, pitch));
    }

    private IEnumerator PlaySequenceCoroutine(AudioClip[] clips, float pitch = 1f)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip == null) continue;

            _audioSource.PlayOneShot(clip, pitch);

            yield return new WaitForSecondsRealtime(clip.length);
        }

    }
}
