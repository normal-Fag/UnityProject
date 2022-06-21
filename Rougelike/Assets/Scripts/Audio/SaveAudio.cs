using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveAudio : MonoBehaviour
{
   [SerializeField] private Slider musicVolume;
   [SerializeField] private Slider fxVolume;
   [SerializeField] private Toggle allSoundsOff;

    private void Start()
    {
        Load();
    }
    public void Save()
    {
        float mVolume = musicVolume.value;
        float FXVolume = fxVolume.value;
        bool allSounds = allSoundsOff.isOn;


        SaveGeneral saveInventory = new SaveGeneral
        {
            musicVolume = mVolume,
            fxVolume = FXVolume,
            allSoundsOff = allSounds,

        };

        string json = JsonUtility.ToJson(saveInventory);

        File.WriteAllText(Application.persistentDataPath + "/Save/options.txt", json);

    }
    public class SaveGeneral
    {
        public float musicVolume;
        public float fxVolume;
        public bool allSoundsOff;

    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Save/options.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/Save/options.txt");
            SaveGeneral saveOption = JsonUtility.FromJson<SaveGeneral>(saveString);
            musicVolume.value = saveOption.musicVolume;
            fxVolume.value = saveOption.fxVolume;
            allSoundsOff.isOn = saveOption.allSoundsOff;
        }
    }

}
