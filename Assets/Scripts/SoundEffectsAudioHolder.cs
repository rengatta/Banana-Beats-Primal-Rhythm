using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsAudioHolder : MonoBehaviour
{

    public List<AudioSource> audioSources;

    public OptionsMenu optionsMenu;

    public int numSources = 5;


    public void SetOptions()
    {

        int muteToggled = PlayerPrefs.GetInt("MuteToggled", 0);
        foreach (AudioSource audioSource in audioSources) {
            audioSource.volume = PlayerPrefs.GetFloat("Volume", 1.0f);
            if (muteToggled == 1)
            {
                audioSource.mute = true;
            }
            else
            {
                audioSource.mute = false;
            }
        }

    }





    void Start()
    {


        if(optionsMenu != null) {
            optionsMenu.volumeChangeEvent.AddListener(SetOptions);
            optionsMenu.muteToggleChangeEvent.AddListener(SetOptions);
        }

        SetOptions();
    }

   
  
}
