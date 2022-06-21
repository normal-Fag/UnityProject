using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterFXload : SaveAudio
{
    private bool muteOn;
    private float fxVolumeCharacter;
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/Save/options.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/Save/options.txt");
            SaveGeneral saveOption = JsonUtility.FromJson<SaveGeneral>(saveString);
            if (saveOption.allSoundsOff)
            {
                muteOn = true;
            }
            else
                fxVolumeCharacter = saveOption.fxVolume;
        }
        foreach (Transform child in transform)
        {
            if (muteOn)
                child.GetComponent<AudioSource>().mute = muteOn;

            child.GetComponent<AudioSource>().volume = fxVolumeCharacter;
        }
    }

   
}
