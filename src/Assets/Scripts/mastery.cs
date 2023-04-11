using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mastery : MonoBehaviour {
    
    private TMP_Text endText;
    private float timeReqToMaster = 10000 * 60 * 60; //at least according to Gladwell 
    private float masteryCalc;

    void Awake() {
        endText = GetComponent<TMP_Text>();

        if ((PlayerPrefs.GetFloat("masteryTime") / timeReqToMaster) >= 1) {
            masteryCalc = 1.0f;
        } else {
            masteryCalc = (PlayerPrefs.GetFloat("masteryTime") / timeReqToMaster);
        }
    }

    void Start() {
        //Debug.Log("masteryTime: " + PlayerPrefs.GetFloat("masteryTime"));
        endText.text += "<size=20%>\n <size=80%>\nLevel of Mastery:\n" + masteryCalc.ToString("f7") + "%";
    }
}
