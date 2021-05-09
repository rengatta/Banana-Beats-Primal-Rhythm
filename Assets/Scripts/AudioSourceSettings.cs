using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioSourceSettings : MonoBehaviour
{
    public bool ignoreListenerPause = false;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = ignoreListenerPause;
    }

}
