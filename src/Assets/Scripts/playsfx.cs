using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playsfx : MonoBehaviour {

    public AudioSource source;
    public AudioClip sfx; //set in object inspector

    public void PlaySFX() {
        source.clip = sfx;
        source.Play();
    }
}
