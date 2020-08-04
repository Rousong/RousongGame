using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
/// <summary>
/// 
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("环境音效")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("Player音效")]
    public AudioClip[] walkStepClips;
    public AudioClip jumpClip;
    public AudioClip jumpVoiceClip;

    [Header("UI音效")]
    public AudioClip OpenInventoryClip;
    public AudioClip CloseInventoryClip;

    //声源
    AudioSource ambientSource;  //环境音
    AudioSource musicSource;    //背景音乐
    AudioSource fxSource;
    AudioSource playerSource;   //玩家脚步声
    AudioSource voiceSource;    //玩家角色声音
    AudioSource inventorySource; //背包的声音

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();
        inventorySource = gameObject.AddComponent<AudioSource>();

        StartLevelAudio();
    }

    void StartLevelAudio()
    {
        //环境音
        instance.ambientSource.clip = ambientClip;
        instance.ambientSource.loop = true; //重复播放
        instance.ambientSource.Play();
        //背景音
        instance.musicSource.clip = musicClip;
        instance.musicSource.loop = true;
        instance.musicSource.Play();
    }

    //播放随机脚步声，注意这里使用单例模式，应该使用instance来访问方法，不要加static
    public void PlayFootstepAudio()
    {
        int index = Random.Range(0, instance.walkStepClips.Length);
        instance.playerSource.clip = instance.walkStepClips[index];
        instance.playerSource.Play();
    }
    //跳跃的动作声和人声
    public void PlayJumpAudio()
    {
        instance.playerSource.clip = instance.jumpClip;
        instance.playerSource.Play();

        instance.voiceSource.clip = instance.jumpVoiceClip;
        instance.voiceSource.Play();
    }

    public void PlayInventoryAudio(bool isOpen)
    {
        if (isOpen)
        {
           instance.inventorySource.clip = instance.CloseInventoryClip;
           instance.playerSource.Play();
        }
        else
        {
            instance.inventorySource.clip = instance.OpenInventoryClip;
            instance.playerSource.Play();
        }
    }
}
