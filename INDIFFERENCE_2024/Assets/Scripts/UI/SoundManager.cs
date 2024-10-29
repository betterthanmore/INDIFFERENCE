using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    // BGM�� SFX ������
    public enum EBgm
    {
        BGM_TITLE,
        BGM_GAME,
    }

    public enum ESfx
    {
        SFX_BUTTON,
        SFX_MISSION_CLEAR
    }

    // ����� Ŭ�� �迭
    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioClip[] sfxs;

    // ����� ����� AudioSource
    [SerializeField] private AudioSource audioBgm;
    [SerializeField] private AudioSource audioSfx;

    // ���� ��� ���� BGM�� ����
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

    // ������� ��� (���� BGM�� �ݺ����� ����)
    public void PlayBGM(EBgm bgmIdx, float volume = 1.0f)
    {
        if (currentBgm == bgmIdx && audioBgm.isPlaying)
            return; // �̹� ���� ��������� ��� ���̸� ������� ����

        audioBgm.clip = bgms[(int)bgmIdx];
        audioBgm.volume = volume;
        audioBgm.loop = true;
        audioBgm.Play();
        currentBgm = bgmIdx;
    }

    // ������� ����
    public void StopBGM()
    {
        audioBgm.Stop();
        currentBgm = null;
    }

    // ȿ���� ���
    public void PlaySFX(ESfx esfx, float volume = 1.0f)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx], volume);
    }

    // ������� ���� ����
    public void SetBGMVolume(float volume)
    {
        audioBgm.volume = volume;
    }

    // ȿ���� ���� ����
    public void SetSFXVolume(float volume)
    {
        audioSfx.volume = volume;
    }
    void Start()
    {
        // ���� ���� �� �ڵ����� Ÿ��Ʋ ������� ���
        PlayBGM(EBgm.BGM_TITLE);
    }
}