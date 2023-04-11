using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class revealwipe : MonoBehaviour {

    public float delayBeforeWipe = 1.0f;
    public float durationOfWipe  = 2.0f;
    private float wipeDistance    = 30.0f;
    private GameObject wipeFlag;

    void Awake() {
        wipeFlag = GameObject.Find("flag");
    }

    void Start() {
        StartCoroutine(WipeLeft(delayBeforeWipe, durationOfWipe));
    }

    IEnumerator WipeLeft(float delay, float duration) {
        yield return new WaitForSeconds(delay);
        float currentTime = 0.0f;
        Vector3 flagPos = wipeFlag.transform.position;

        while (currentTime < duration) {
            float xPos = Mathf.Lerp(flagPos.x, (flagPos.x - wipeDistance), currentTime / duration);
            wipeFlag.transform.position = new Vector3 (xPos, flagPos.y, flagPos.z);
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject.transform.parent.gameObject);

        yield break;
    }

}