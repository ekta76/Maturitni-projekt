using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sFXSource;
    [SerializeField] AudioSource musicSource;

    public AudioClip music;
    public AudioClip footstep;
    public AudioClip slash;
    public AudioClip slimeHit;
    public AudioClip skeletonHit;
    public AudioClip zombieHit;
    public AudioClip ratHit;
    public AudioClip minotaurHit;
    public AudioClip gettingHitNormal;
    public AudioClip gettingHitFireball;
    public AudioClip itemPickUp;
    public AudioClip healing;

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
