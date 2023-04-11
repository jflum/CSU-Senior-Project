using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxpref : MonoBehaviour {
    
    public AudioSource source;
    private bool audioEnabled;

    void Start() {
        audioEnabled = (PlayerPrefs.GetInt("sfx", 1) == 0) ? source.mute = true : source.mute = false;
    }

    public void UpdatePref() {
        audioEnabled = (PlayerPrefs.GetInt("sfx") == 0) ? source.mute = true : source.mute = false;
        if (audioEnabled) source.volume = 1.0f;
    }
}