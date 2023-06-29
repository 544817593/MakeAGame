using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class MusicSlider : MonoBehaviour
{
    [SerializeField]
     Slider m_slider;

    [SerializeField]
   Slider s_slider;
    private void Start()
    {
       
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadMusic();
            LoadSound();
        }
        else if(!PlayerPrefs.HasKey("soundVolume"))
            {
            PlayerPrefs.SetFloat("soundVolume", 1);
            LoadMusic();
            LoadSound();
        }
        else
        {
            LoadMusic();
            LoadSound();
        }
    }

    public void ChangeMusicVolume()
    {
        AudioKit.Settings.MusicVolume.Value = m_slider.value;
                
       
    }
    public void ChangeSoundVolume()
    {
        AudioKit.Settings.SoundVolume.Value = s_slider.value;
    }    
    public void LoadMusic()
    {
        m_slider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    public void LoadSound()
    {
        s_slider.value = PlayerPrefs.GetFloat("soundVolume");
    }
    public void SaveMusic()
    {
        PlayerPrefs.SetFloat("musicVolume", m_slider.value);
    }
    public void SaveSound()
    {
        PlayerPrefs.SetFloat("soundVolume", s_slider.value);
    }
}
