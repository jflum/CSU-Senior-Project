using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clearprefs : MonoBehaviour {
    
    private Button resetButton;
    
    void Awake() {
        resetButton = GameObject.Find("clearprefs").GetComponent<Button>();
    }

    public void ClearAllPlayerPrefs() {
        //EXT: warn before executing
        PlayerPrefs.DeleteAll();
        resetButton.interactable = false;
        playersettings settings = GameObject.Find("settings").GetComponent<playersettings>();
        settings.SetGameMode();
        settings.SetLevelSelect();
        settings.SetBGMPref();
        settings.SetSFXPref();

    }
}
