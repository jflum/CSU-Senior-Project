using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoresfx : MonoBehaviour {
    
    public GameObject popup;
    public AudioSource source;
    public AudioClip sfx; //set in object inspector

    public void PlaySFX() {

        //NOTE: specific use case for preventing sfx on refocus of popup if already on screen, without custom raycast
        if (popup.transform.localPosition.x != 200) {
            source.clip = sfx;
            source.Play();
        }
    }
}
