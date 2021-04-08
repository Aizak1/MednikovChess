using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioClip pickSound;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioClip killSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPicKSound()
    {
        audioSource.PlayOneShot(pickSound);
    }
    public void PlayDropSound()
    {
        audioSource.PlayOneShot(dropSound);
    }
    public void PlayKillSound()
    {
        audioSource.PlayOneShot(killSound);
    }
}
