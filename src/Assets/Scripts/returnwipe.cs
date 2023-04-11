using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class returnwipe : MonoBehaviour {

    public float delayBeforeWipe = 1.0f;
    public float durationOfWipe  = 2.0f;
    private float wipeDistance    = 30.0f;
    private GameObject wipeFlag;
    private AudioSource source;

    void Awake() {
        wipeFlag = GameObject.Find("flag");
        source = GameObject.Find("bgm").GetComponent<AudioSource>();
    }

    void Start() {
        StartCoroutine(WipeLeft(delayBeforeWipe, durationOfWipe));
    }

    IEnumerator WipeLeft(float delay, float duration) {
        yield return new WaitForSeconds(delay);
        float currentTime = 0.0f;
        float currentVolume = source.volume;
        Vector3 flagPos = wipeFlag.transform.position;

        while (currentTime < duration) {
            float xPos = Mathf.Lerp(flagPos.x, (flagPos.x - wipeDistance), currentTime / duration);
            wipeFlag.transform.position = new Vector3 (xPos, flagPos.y, flagPos.z);

            float vol = Mathf.Lerp(currentVolume, 0.0f, currentTime / duration);
            source.volume = vol;

            currentTime += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("Start");
        yield break;
    }

}
