using System;
using UnityEngine;

public enum BGMType
{
    None = -1,
}

public enum SFXType
{
    None = -1,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private BGMType currentBGMClip = BGMType.None;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] bgmClips;
    [SerializeField] private AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public float BGMVolume
    {
        get => bgmSource.volume;
        set => bgmSource.volume = value;
    }
    
    public float SFXVolume
    {
        get => sfxSource.volume;
        set => sfxSource.volume = value;
    }

    public void PlayBGM(BGMType type)
    {
        if (type == BGMType.None) return; 
        
        bgmSource.clip = bgmClips[(int) type];
        bgmSource.Play();

        currentBGMClip = type;
    }

    public void StopBGM()
    {
        bgmSource.Stop();

        currentBGMClip = BGMType.None;
    }

    public void PlaySFX(SFXType type)
    {
        if (type == SFXType.None) return; 

        sfxSource.clip = sfxClips[(int) type];
        sfxSource.Play();
    }
}