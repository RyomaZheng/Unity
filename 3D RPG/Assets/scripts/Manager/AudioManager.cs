using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    AudioSource bgmSource;
    AudioSource effectSource;
    AudioSource enemySource;
    AudioSource itemSource;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        effectSource = gameObject.AddComponent<AudioSource>();
        effectSource.playOnAwake = false;
        enemySource = gameObject.AddComponent<AudioSource>();
        enemySource.playOnAwake = false;
        itemSource = gameObject.AddComponent<AudioSource>();
        itemSource.playOnAwake = false;
    }

    private void PlayMusic(AudioClip ac, string audioName, bool loop)
    {
        AudioSource temp = null;
        switch (audioName)
        {
            case "Bgm":
                temp = bgmSource;
                break;
            case "Effect":
                temp = effectSource;
                break;
            case "Enemy":
                temp = enemySource;
                break;
            case "Item":
                temp = itemSource;
                break;
        }
        if (temp.isPlaying)
        {
            temp.Stop();
        }
        temp.clip = ac;
        temp.loop = loop;
        temp.volume = 0.2f;
        temp.Play();
    }

    public void PlayMusicByName(string name, bool loop)
    {
        string audioName = name.Substring(0, name.IndexOf("/"));
        AudioClip ac = Resources.Load<AudioClip>("Audio/" + name);
        PlayMusic(ac, audioName, loop);
    }
}
