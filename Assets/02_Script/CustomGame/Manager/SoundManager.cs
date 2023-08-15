using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundLs
{

    public string name;
    public AudioClip sorce;
    public bool loop, awake;
    [Range(0, 1)] public float volume = 1;

}

public class SoundManager : MonoBehaviour
{

    [SerializeField] private List<SoundLs> soundList = new List<SoundLs>();

    private Dictionary<string, AudioSource> audioDic = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        
        foreach(var sound in soundList)
        {

            GameObject go = new GameObject(sound.name);

            var aud = go.AddComponent<AudioSource>();
            aud.playOnAwake = sound.awake;
            aud.clip = sound.sorce; 
            aud.loop = sound.loop;
            aud.volume = sound.volume;

            go.transform.SetParent(transform);

            audioDic.Add(sound.name, aud);

        }

    }

    public void PlaySound(string name)
    {

        audioDic[name].Play();

    }

    public void PauseSound(string name)
    {

        audioDic[name].Pause();

    }

}
