using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ready : MonoBehaviour {

    public float delayBeforeLoad = 1.0f;

    void Start() {
        StartCoroutine(Wait(delayBeforeLoad));
    }

    IEnumerator Wait(float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Game Loop");
        yield break;
    }

}
