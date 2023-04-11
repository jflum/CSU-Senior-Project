using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class popupmenu : MonoBehaviour {

    public GameObject popup;
    private TMP_InputField input = null;
    private Vector3 offScreenPos = new Vector3(1200, 120, 0);
    private Vector3 onScreenPos = new Vector3(200, 120, 0);
    private bool dismissed = false;

    void Awake() {
        if (GameObject.Find("input") != null) {
            input = GameObject.Find("input").GetComponent<TMP_InputField>();
        }
    }

    void Update() {
        if (input != null && input.isFocused && !dismissed) {
            input.DeactivateInputField();
        }
    }

    public void OpenPopup() {
        popup.transform.localPosition = onScreenPos;
    }

    public void ClosePopup() {
        popup.transform.localPosition = offScreenPos;
        
        if (input != null) {
            dismissed = true;
            input.Select();
            input.ActivateInputField();
        }
    }
}
