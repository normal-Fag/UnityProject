using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    public AudioMixer musicMixer;
    [SerializeField]
    public Toggle audioToggle;

    public void setMusicVolume(float musicVolume)
    {
        musicMixer.SetFloat("musVolume", musicVolume);
    }
    public void setFxVolume(float fxVolume)
    {
        musicMixer.SetFloat("fxVolume", fxVolume);
    }
    public void offSound(bool off)
    {
        if (off)
        {
            musicMixer.SetFloat("master", -80);
        }
        else
        {
            musicMixer.SetFloat("master", 0);
        }
    }
}
