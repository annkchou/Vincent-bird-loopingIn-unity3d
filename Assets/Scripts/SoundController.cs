using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    public enum Channel { Music, Sounds, Master }
    [Serializable]
    private struct AudioProperties
    {
        public Channel channel;
        public string name;
        public float volume;
        public bool muted;
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioProperties[] audioProperties;
    private AudioProperties empty = new AudioProperties();
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private bool willLoop;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        //Set up references
        source = GetComponent<AudioSource>();
        //Register Callbacks
       // GameManager.instance.GameStartedEvent += PlayMusic;
       // GameManager.instance.GameOverEvent += StopMusic;
       // UIManager.AudioChangeEvent += AdjustAudio;
    }
    //Callback Methods
    private void PlayMusic()
    {
        source.clip = backgroundMusic;
        source.loop = willLoop;
        source.Play();
    }
    private void StopMusic()
    {
        source.Pause();
    }

    private void AdjustAudio(Channel channel, float volume, bool isMuted)
    {
        //convert (0.0 to 1.0) float to (-80db to 20db)
        float db = (volume * 100.0f) - 80.0f;
        //get the exposed volume parameter name
        AudioProperties audio = GetGroupFor(channel);
        if (audio.name == empty.name)
        {
            return;
        }
        //set values
        audio.volume = volume;
        audio.muted = isMuted;

        if (isMuted)
        {
            db = -80.0f;
        }

        //set the exposed volume
        mixer.SetFloat(audio.name, db);

    }

    private AudioProperties GetGroupFor(Channel channel)
    {
        foreach (var prop in audioProperties)
        {
            if (prop.channel == channel)
            {
                return prop;
            }
        }
        return empty;
    }
}

