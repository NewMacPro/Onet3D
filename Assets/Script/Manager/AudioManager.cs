using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AudioManager : UnitySingleton<AudioManager>
{

    public const string MAIN_BGM = "Sounds/music/main_BGM"; //大厅的bgm
    public const string PLAY_BGM = "Sounds/Game/play_BGM"; //打牌时的bgm
    public const string BUTTON_SOUND = "Sounds/music/button"; //button音效

    public AudioSource effectSource;
    public AudioSource effectSource1;
    public AudioSource musicSource;

    private List<AudioSource> audioSourceList;

    public float low_pitch_range = 0.95f;
    public float high_pitch_range = 1.05f;

    public bool enableBGM = false;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        audioSourceList = new List<AudioSource>(this.GetComponents<AudioSource>());
        if (audioSourceList.Count == 0)
        {
            audioSourceList.Add(gameObject.AddComponent<AudioSource>());
            audioSourceList.Add(gameObject.AddComponent<AudioSource>());
            audioSourceList.Add(gameObject.AddComponent<AudioSource>());
        }

        musicSource = audioSourceList[0];
        effectSource = audioSourceList[1];
        effectSource1 = audioSourceList[2];

        musicSource.loop = true;
    }


    public void SetMusicPitch(float pitch)
    {
        musicSource.pitch = pitch;
    }

    public void PlayButtonSound()
    {
        //PyLuaClient.CallFuntion("AudioManager.PlayButton");
    }

    public void PlayButtonSound(System.Action action)
    {
        PlayButtonSound();
        CoroutineHelper.Instance.WaitForSeconds(1, delegate
        {
            action();
        });
    }

    public void PlaySingle(AudioClip clip)
    {
        PlaySingle(clip, 1);
    }

    public AudioClip PlaySingle(string audioPath)
    {
        AudioClip clip = Instantiate(iResourceManager.Load<AudioClip>(audioPath));
        if (clip == null)
        {
            Debug.Log("声音数据没有读取到：" + audioPath);
            return null;
        }
        PlaySingle(clip);
        return clip;
    }

    public void PlaySingle(AudioClip clip, float volume)
    {
        if (!LocalSave.SoundOn())
            return;

        AudioSource playSource = effectSource;
        if (effectSource.isPlaying)
            playSource = effectSource1;

        if (volume > 1f || volume < 0f)
            volume = 1f;

        playSource.clip = clip;
        playSource.volume = volume;
        playSource.Play();
    }

    public void PlayBGM(string bgmPath)
    {
        if (!enableBGM)
            return;

        AudioClip clip = iResourceManager.Load<AudioClip>(bgmPath);
        if (musicSource.clip == null || musicSource.clip.name != clip.name)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        SetMusicOn(LocalSave.MusicOn());
    }

    public void SetMusicOn(bool music_on)
    {
        LocalSave.SetMusic(music_on);
        musicSource.mute = !music_on;
    }

    public void SetSoundOn(bool sound_on)
    {
        LocalSave.SetSound(sound_on);
        effectSource.mute = !sound_on;
        effectSource1.mute = !sound_on;
    }

    public bool isAnySoundOn()
    {
        // return !effectSource.mute;
        foreach (var audio in audioSourceList)
        {
            if (!audio.mute)
                return true;
        }

        return false;
    }

    public void SetAudioOn(bool value)
    {
        if (!value)
        {
            musicSource.mute = true;
            effectSource.mute = true;
            effectSource1.mute = true;
        }
        else
        {
            SetMusicOn(LocalSave.MusicOn());
            SetSoundOn(LocalSave.SoundOn());
        }

        LocalSave.SetAudio(value);
    }

    public void PlayAList(List<string> soundsList)
    {
        StartCoroutine(playEngineSound(soundsList));
    }

    IEnumerator playEngineSound(List<string> soundsList)
    {
        for (int i = 0; i < soundsList.Count; i++)
        {
            Debug.Log(soundsList[i]);

            AudioClip sourceClip = iResourceManager.Load<AudioClip>("Sounds/" + soundsList[i]);

            float waitTime = 0f;
            if (sourceClip == null)
            {
                Debug.LogWarning("No Sound file!!:" + soundsList[i]);
            }
            else
            {
                AudioClip clip = Instantiate(sourceClip);
                PlaySingle(clip);
                waitTime = clip.length;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void Randomize_sound_fax(params AudioClip[] clips)
    {
        int random_index = Random.Range(0, clips.Length);
        float random_pitch = Random.Range(low_pitch_range, high_pitch_range);

        effectSource.pitch = random_pitch;
        effectSource.clip = clips[random_index];
        effectSource.Play();
    }

}
