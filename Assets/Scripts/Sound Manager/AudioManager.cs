using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance { get { return instance; } }

    [SerializeField] private SoundType[] background, effects;
    [SerializeField] private AudioSource[] sources;

    [Serializable]
    public class SoundType
    {
        public BackgroundSound bgm;
        public Effects sfx;
        public AudioClip clip;
    }

    public enum BackgroundSound
    {
        Menu,
        Gameplay,
    }

    public enum Effects
    {
        Grow,
        Reduce,
        Speed,
        Phase,
        Double,
        Burn
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(Effects effect)
    {
        AudioClip clip = getSoundClip(effect);
        if (clip != null)
        {
            sources[0].PlayOneShot(clip);
        }
    }

    public void PlayBGM (BackgroundSound sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            sources[1].clip = clip;
            sources[1].Play();
        }
    }

    private AudioClip getSoundClip(Enum soundType)
    {
        if (soundType is BackgroundSound)
        {
            BackgroundSound bgmType = (BackgroundSound)soundType;
            SoundType bgmItem = Array.Find(background, item => item.bgm == bgmType);
            if (bgmItem != null)
            {
                return bgmItem.clip;
            }
        }
        else if (soundType is Effects)
        {
            Effects sfxType = (Effects)soundType;
            SoundType sfxItem = Array.Find(effects, item => item.sfx == sfxType);
            if (sfxItem != null)
            {
                return sfxItem.clip;
            }
        }

        return null;
    }

}
