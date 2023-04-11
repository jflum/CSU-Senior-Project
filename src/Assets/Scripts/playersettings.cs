using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playersettings : MonoBehaviour {

    public GameObject start;
    public AudioSource source;
    private Button startButton;
    private Toggle bgmToggle;
    private Toggle sfxToggle;
    private Toggle breakToggle;
    private TMP_Dropdown gameMode;
    private TMP_Text gameModeDesc;
    private Slider levelSelect;
    private TMP_Text levelSelectDesc;
    private Color32 warn = new Color32(255, 144, 144, 255);
    private bool togEnabled;
    
    private const float UNMUTE_DELAY = 1.0f;

//----------------------------------------------------------------------------------------------------------------------

    void Awake () {   
        startButton = start.GetComponent<Button>();

        levelSelect = GameObject.Find("levelSelectSlider").GetComponent<Slider>();
        levelSelect.value = PlayerPrefs.GetInt("levelSelect", 1);
        UpdateLevelSelectDesc();

        gameMode = GameObject.Find("gameModeDropDown").GetComponent<TMP_Dropdown>();
        gameMode.value = PlayerPrefs.GetInt("gameMode", 0); 
        UpdateGameModeDesc();

        bgmToggle = GameObject.Find("bgmTog").GetComponent<Toggle>();
        togEnabled = PlayerPrefs.GetInt("bgm", 1) == 1 ? bgmToggle.isOn = true : bgmToggle.isOn = false;
        SetBGMPref();

        sfxToggle = GameObject.Find("sfxTog").GetComponent<Toggle>();
        togEnabled = PlayerPrefs.GetInt("sfx", 1) == 1 ? sfxToggle.isOn = true : sfxToggle.isOn = false;
        SetSFXPref();

        breakToggle = GameObject.Find("breakTog").GetComponent<Toggle>();
        togEnabled = PlayerPrefs.GetInt("breaknag", 1) == 1 ? breakToggle.isOn = true : breakToggle.isOn = false;
        SetBreakPref();

        PlayerPrefs.Save();

        if (PlayerPrefs.GetInt("sfx") == 1) {
            StartCoroutine(sfxEnableDelay(UNMUTE_DELAY)); //prevents sfx from onValueChange() during user pref load in

        }
    }

//----------------------------------------------------------------------------------------------------------------------

    public void SetGameMode() {
        PlayerPrefs.SetInt("gameMode", gameMode.value);
        PlayerPrefs.Save();
        UpdateGameModeDesc();
    }

    public void UpdateGameModeDesc() {
        gameModeDesc = GameObject.Find("gameModeDesc").GetComponent<TMP_Text>();
        switch (gameMode.value) {
            case 0:
                gameModeDesc.text = "Difficulty increases over time, commensurate with player performance.";
                gameModeDesc.color = Color.white;
                startButton.interactable = true;
                break;
            case 1:
                gameModeDesc.text = "Complexity remains constant, however time becomes increasly limited.";
                gameModeDesc.color = Color.white;
                startButton.interactable = true;
                break;
            case 2:
                gameModeDesc.text = "Focus on improving accuracy with no time constraints. Scoring is disabled.";
                gameModeDesc.color = Color.white;
                startButton.interactable = true;
                break;
            case 3:
                gameModeDesc.text = "Educator's Edition Pro License is required for custom configurations.";
                gameModeDesc.color = warn;
                startButton.interactable = false;
                break;
            default:
                Debug.Log("game mode dropdown out of intended range");
                Debug.Break();
                break;
        }
    }

    public void SetLevelSelect() {
        PlayerPrefs.SetInt("levelSelect", (int)levelSelect.value);
        PlayerPrefs.Save();
        UpdateLevelSelectDesc();
    }

    public void UpdateLevelSelectDesc() {
        levelSelectDesc = GameObject.Find("levelSelectDesc").GetComponent<TMP_Text>();
        switch (levelSelect.value) {
            case 1:
                levelSelectDesc.text = "NOVICE. Minimal complexity and pressure; a good place to start.";
                break;
            case 2:
                levelSelectDesc.text = "INTERMEDIATE. Medium complexity and a generous time allotment.";
                break;
            case 3:
                levelSelectDesc.text = "EXPERIENCED. High complexity with less forgiving time contraints.";
                break;
            case 4:
                levelSelectDesc.text = "ADVANCED. Full complexity with additional modifiers and a strict timer.";
                break;
            case 5:
                levelSelectDesc.text = "EXPERT. For those who arduously seek Mastery...";
                break;
            default:
                Debug.Log("level select slider out of intended range");
                Debug.Break();
                break;
        }
    }

    public void SetBGMPref() {
        PlayerPrefs.SetInt("bgm", (bgmToggle.isOn == true ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetSFXPref() {
        PlayerPrefs.SetInt("sfx", (sfxToggle.isOn == true ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetBreakPref() {
        PlayerPrefs.SetInt("breaknag", (breakToggle.isOn == true ? 1 : 0));
        PlayerPrefs.Save();
    }
    
//----------------------------------------------------------------------------------------------------------------------

    IEnumerator sfxEnableDelay(float delay) {
        yield return new WaitForSeconds(delay);
        source.volume = 1.0f;
        yield break;
    }
}
