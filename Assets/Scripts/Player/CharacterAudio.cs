using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CharacterAudio : MonoBehaviour
{
    [SerializeField] AudioSource mainEffectAudioSouce = null;
    [Header("Audio Sources")]
    [SerializeField] AudioSource jumpAudioSource = null;
    [SerializeField] AudioSource footstepsRunAudioSource = null;
    [SerializeField] AudioSource dashAudioSource = null;

    public enum AudioType
    {
        HardLanding, Jump, TakeHit, Landing, BackDash, Dash, Falling, FootstepsRun, FoorstepsWalk,
        WallSlide, NailArtCharge, NailArtReady, SlugWalk, SuperDashing, SuperDashCharge, DreamNail,
        Swim, WallJump, HeroDamage, HeroWings,
    }

    public void Play(AudioType audioType, bool playState)
    {
        AudioSource audioSource = null;
        switch (audioType)
        {
            case AudioType.Jump:
                audioSource = jumpAudioSource;
                break;
            case AudioType.FootstepsRun:
                audioSource = footstepsRunAudioSource;
                break;
            case AudioType.Dash:
                audioSource = dashAudioSource;
                break;
        }
        if (audioSource != null)
        {
            if (playState)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

 }
