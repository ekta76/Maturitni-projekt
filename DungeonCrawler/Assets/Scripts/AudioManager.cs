using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource sFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip music;
    public AudioClip footstep;
    public AudioClip slash;
    public AudioClip slimeHit;

    private void Start()
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sFXSource.PlayOneShot(clip);
    }
}
