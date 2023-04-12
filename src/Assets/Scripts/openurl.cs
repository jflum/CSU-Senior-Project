using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class openurl : MonoBehaviour {
    
    public AudioSource sfxSource;

    public void OpenURL(string url) {
        StartCoroutine(WaitForSFX(url));
    }

    IEnumerator WaitForSFX(string url) {
        //yield return new WaitWhile (()=> sfxSource.isPlaying);
        yield return new WaitForSeconds(sfxSource.clip.length - 0.75f);
        EventSystem.current.SetSelectedGameObject(null);
        Application.OpenURL(url);
    }
}