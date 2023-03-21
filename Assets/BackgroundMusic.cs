using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _audioClips;

    private void Start()
    {
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {
        yield return new WaitUntil(() => !_audioSource.isPlaying);
        _audioSource.PlayOneShot(_audioClips[Random.Range(0, _audioClips.Count - 1)]);
    }

}
