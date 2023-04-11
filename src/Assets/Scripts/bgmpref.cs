using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmpref : MonoBehaviour {
    
    //QUIRK: Need to interact once with browser window before music plays. Chrome limitation. See Unity reference
    public AudioSource source;
    private bool audioEnabled;

    void Start() {
        audioEnabled = (PlayerPrefs.GetInt("bgm", 1) == 0) ? source.mute = true : source.mute = false;
    }

    public void UpdatePref() {
        audioEnabled = (PlayerPrefs.GetInt("bgm") == 0) ? source.mute = true : source.mute = false;
        source.Stop(); source.Play();
    }
}
