using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class start : MonoBehaviour {
    
    public GameObject wipe;
    public SpriteRenderer border;
    private Color temp;

    public void StartGame() {
        PlayerPrefs.SetFloat("breaktime", 0);
        GameObject newWipe = Instantiate(wipe, new Vector3(0, 0, 0), Quaternion.identity);
        newWipe.name = "wipe";
    }

    public void OnEnter() {
        //Debug.Log("on enter hover start button");
        temp = border.color;
        temp.a = 1.0f;
        border.color = temp;
    }

    public void OnExit() {
        //Debug.Log("on exit hover start button");
        temp = border.color;
        temp.a = 0.0f;
        border.color = temp;
    }
}
