using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioSource music;
    public AudioSource fx;
    [SerializeField]
    public Slider musicMixer;
    public Slider fxMixer;
    [SerializeField]
    public Toggle audioToggle;

    public void setMusicVolume()
    {
        music.volume = musicMixer.value;
    }
    public void setFxVolume()
    {
        fx.volume = fxMixer.value;
    }
    public void offSound(bool off)
    {
        if (off)
        {
            music.mute = true;
            fx.mute = true;
        }
        else
        {
            music.mute = false;
            fx.mute = false;
        }
    }
}
