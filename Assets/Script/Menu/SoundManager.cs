using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource sound_BGM;
    public AudioSource sound_Button;
    public Slider musicVolumeSlider; // ��� ���� ������ �����ϴ� �����̴�
    public Slider effectVolumeSlider; // ��ư ȿ���� ������ �����ϴ� �����̴�
    private float originalBGMVolume; // ���� ��� ���� ���� ���� ������ ����
    private float originalButtonVolume; // ���� ��ư ȿ���� ���� ���� ������ ����

    private void Start()
    {
        if (PlayerPrefs.HasKey("BGMVolume") && PlayerPrefs.HasKey("ButtonVolume"))
        {
            float bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
            sound_BGM.volume = bgmVolume;
            float buttonVolume = PlayerPrefs.GetFloat("ButtonVolume");
            sound_Button.volume = buttonVolume;
        }


        originalBGMVolume = sound_BGM.volume;
        originalButtonVolume = sound_Button.volume;
        musicVolumeSlider.value = originalBGMVolume;
        effectVolumeSlider.value = originalButtonVolume;
    }


    public void SetBgmVolume(float volume)
    {
        sound_BGM.volume = volume;
        musicVolumeSlider.value = volume;
    }

    public void SetEffectVolume(float volume)
    {
        sound_Button.volume = volume;
        effectVolumeSlider.value = volume;
    }


    public void OnSoundButton()
    {
        sound_Button.Play();
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", sound_BGM.volume);
        PlayerPrefs.SetFloat("ButtonVolume", sound_Button.volume);
    }

    public void LoadSoundSettings()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            float bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
            sound_BGM.volume = bgmVolume;
            musicVolumeSlider.value = bgmVolume;
        }

        if (PlayerPrefs.HasKey("ButtonVolume"))
        {
            float buttonVolume = PlayerPrefs.GetFloat("ButtonVolume");
            sound_Button.volume = buttonVolume;
            effectVolumeSlider.value = buttonVolume;
        }
    }
}