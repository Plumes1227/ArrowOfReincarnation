using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] public AudioMixer AudioMixerObj;
    [SerializeField] AudioSource sFXPlayer;

    public float bgmSliderValue = 0.8f;
    public float sfxSliderValue = 0.8f;
    public bool bgmToggleBool = true;
    public bool sfxToggleBool = true;

    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    // public void PlayRandomSFX(AudioData audioData)
    // {
    //     sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
    //     PlaySFX(audioData);
    // }

    // public void PlayRandomSFX(AudioData[] audioData)
    // {
    //     PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    // }
}
