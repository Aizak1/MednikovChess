using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] tracks;
    private AudioSource audioSource;

    private static MusicPlayer musicPlayer;
    private void Awake()
    {
        if (musicPlayer == null)
        {
            musicPlayer = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(tracks[Random.Range(0, tracks.Length)]);
    }
}
