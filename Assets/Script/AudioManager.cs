using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] musicClips;
    public Sound[] sfxClips;

    private Dictionary<string, AudioClip> musicDictionary;
    private Dictionary<string, AudioClip> sfxDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionaries()
    {
        musicDictionary = new Dictionary<string, AudioClip>();
        foreach (Sound sound in musicClips)
        {
            if (!musicDictionary.ContainsKey(sound.name))
            {
                musicDictionary.Add(sound.name, sound.clip);
            }
        }

        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (Sound sound in sfxClips)
        {
            if (!sfxDictionary.ContainsKey(sound.name))
            {
                sfxDictionary.Add(sound.name, sound.clip);
            }
        }
    }

    public void PlayMusic(string name)
    {
        if (musicDictionary.ContainsKey(name))
        {
            musicSource.clip = musicDictionary[name];
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySoundEffect(string name)
    {
        if (sfxDictionary.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sfxDictionary[name]);
        }
    }
}
