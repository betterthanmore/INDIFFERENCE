using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // BGM과 SFX 종류들
    public enum EBgm
    {
        BGM_TITLE,
        BGM_GAME,
    }

    public enum ESfx
    {
        SFX_JUMP,
        SFX_WALK,
        SFX_RUN,
        SFX_BUTTON
    }

    // 오디오 클립 배열
    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioClip[] sfxs;

    // 재생에 사용할 AudioSource
    [SerializeField] private AudioSource audioBgm;
    [SerializeField] private AudioSource audioSfx;

    public AudioClip GetSfx(SoundManager.ESfx sfx)
    {
        return sfxs[(int)sfx];
    }

    public AudioSource AudioSfx => audioSfx; // 프로퍼티 추가

    // 현재 재생 중인 BGM을 추적
    private EBgm? currentBgm;

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

    // 배경음악 재생 (같은 BGM을 반복하지 않음)
    public void PlayBGM(EBgm bgmIdx, float volume = 1.0f)
    {
        if (currentBgm == bgmIdx && audioBgm.isPlaying)
            return; // 이미 같은 배경음악이 재생 중이면 재생하지 않음

        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.volume = volume;
        audioBgm.loop = true;
        audioBgm.Play();
        currentBgm = bgmIdx;
    }

    // 배경음악 정지
    public void StopBGM()
    {
        audioBgm.Stop();
        currentBgm = null;
    }

    // 효과음 재생
    public void PlaySFX(ESfx esfx, float volume = 1.0f)
    {
        Debug.Log($"PlaySFX called with {esfx}");
        audioSfx.PlayOneShot(sfxs[(int)esfx], volume);
    }

    // 배경음악 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        audioBgm.volume = volume;
    }

    // 효과음 볼륨 조절
    public void SetSFXVolume(float volume)
    {
        audioSfx.volume = volume;
    }
    void Start()
    {
        // 게임 시작 시 자동으로 타이틀 배경음악 재생
        PlayBGM(EBgm.BGM_TITLE);
    }
}