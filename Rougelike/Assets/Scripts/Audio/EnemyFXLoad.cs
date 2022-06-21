using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyFXLoad : SaveAudio
{
    private bool muteOnEnemy;
    private float fxVolumeEnemy;
    private void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/Save/options.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/Save/options.txt");
            SaveGeneral saveOption = JsonUtility.FromJson<SaveGeneral>(saveString);
            if (saveOption.allSoundsOff)
            {
                muteOnEnemy = true;
            }
            else
                fxVolumeEnemy = saveOption.fxVolume;

            GetComponent<AudioSource>().mute = muteOnEnemy;
            GetComponent<AudioSource>().volume = fxVolumeEnemy;
        }

      
       
    }
}
