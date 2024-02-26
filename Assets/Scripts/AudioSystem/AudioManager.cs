using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I { get; private set; }

    [SerializeField] private bool playSceneMusic;
    [SerializeField] private SoundName sceneMusicClip;

    [SerializeField] private bool playAmbianceSound;
    [SerializeField] private SoundName ambianceClip;

    [SerializeField] SoundLibrary soundLibrary;


    private Dictionary<SoundName, float> soundTimers;
    private readonly AudioSource sceneMusicAudioSource;
    private readonly AudioSource ambianceAudioSource;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            // DontDestroyOnLoad(gameObject);
        }
        //else if (I != this)
        //{
        //    Destroy(gameObject);
        //}

        InitializeSoundTimers();
    }

    private void Start()
    {
        if (playSceneMusic)
            PlayBackgroundSounds(sceneMusicClip, sceneMusicAudioSource);
        if (playAmbianceSound)
            PlayBackgroundSounds(ambianceClip, ambianceAudioSource);
    }

    public void PlaySound(SoundName soundName)
    {
        SoundClip soundClip = GetAudioClip(soundName);
        if (soundClip == null || !CanPlaySound(soundClip))
            return;



        /////////////////
        GameObject soundGameObject = new("2D Sound");
        soundGameObject.transform.SetParent(transform);

        AudioSource audioSource2d = soundGameObject.AddComponent<AudioSource>();
        audioSource2d.loop = soundClip.loop;
        audioSource2d.volume = soundClip.volume;

        if (audioSource2d.loop)
        {
            audioSource2d.clip = soundClip.audioClip;
            audioSource2d.Play();
        }
        else
        {
            audioSource2d.PlayOneShot(soundClip.audioClip);
            Destroy(soundGameObject, soundClip.audioClip.length);
        }
    }

    public void PlaySound(SoundName soundName, Vector3 position)
    {
        SoundClip soundClip = GetAudioClip(soundName);
        if (soundClip == null || !CanPlaySound(soundClip))
            return;

        GameObject soundGameObject = new("3D Sound");
        soundGameObject.transform.position = position;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = soundClip.loop;
        audioSource.volume = soundClip.volume;
        audioSource.spatialBlend = soundClip.spacialBlend;

        if(audioSource.loop)
        {
            audioSource.clip = soundClip.audioClip;
            audioSource.Play();
        }else
        {
            audioSource.PlayOneShot(soundClip.audioClip);
            Destroy(soundGameObject, soundClip.audioClip.length);
        }
    }

    public void PlaySound(SoundName soundName, Transform parent)
    {
        SoundClip soundClip = GetAudioClip(soundName);
        if (soundClip == null || !CanPlaySound(soundClip))
            return;

        GameObject soundGameObject = new("3D Sound");
        soundGameObject.transform.SetParent(parent);
        soundGameObject.transform.localPosition = Vector3.zero;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = soundClip.loop;
        audioSource.volume = soundClip.volume;
        audioSource.spatialBlend = soundClip.spacialBlend;

        if (audioSource.loop)
        {
            audioSource.clip = soundClip.audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(soundClip.audioClip);
            Destroy(soundGameObject, soundClip.audioClip.length);
        }
    }

    public void PlayBackgroundSounds(SoundName soundName, AudioSource audiosource)
    {
        SoundClip soundClip = GetAudioClip(soundName);
        if (soundClip == null || !CanPlaySound(soundClip))
            return;

        if (audiosource == null)
        {
            audiosource = gameObject.AddComponent<AudioSource>();
        }

        audiosource.loop = true;
        audiosource.volume = soundClip.volume;
        audiosource.clip = soundClip.audioClip;

        audiosource.Play();
    }

    public void StopBackGroundSounds()
    {
        if(sceneMusicAudioSource != null)
            sceneMusicAudioSource.Stop();

        if(ambianceAudioSource != null)
            ambianceAudioSource.Stop();
    }

    private void InitializeSoundTimers()
    {
        soundTimers = new Dictionary<SoundName, float>();
        //foreach (SoundClip soundClip in soundLibrary.soundClips)
        //{
        //    if (soundClip.hasPlayTimer)
        //        soundTimers[soundClip.soundName] = soundClip.playTimer;
        //}
    }

    private bool CanPlaySound(SoundClip soundClip)
    {
        if (!soundClip.hasPlayTimer || !soundTimers.ContainsKey(soundClip.soundName))
        {
            if (soundClip.hasPlayTimer)
                soundTimers[soundClip.soundName] = Time.time;

            return true;
        }
            
        
        float lastTimePlayed = soundTimers[soundClip.soundName];
        if (Time.time > lastTimePlayed + soundClip.playTimer)
        {
            soundTimers[soundClip.soundName] = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    private SoundClip GetAudioClip(SoundName soundName)
    {
        foreach (SoundClip soundClip in soundLibrary.soundClips)
        {
            if (soundClip.soundName.Equals(soundName))
            {
                return soundClip;
            }
        }
        return null;
    }
}