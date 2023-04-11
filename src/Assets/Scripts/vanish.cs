using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class vanish : MonoBehaviour {

    private float delayBeforeFade = 0.0f;
    private float durationOfFade  = 1.0f;
    private TMP_Text currIDText;

//----------------------------------------------------------------------------------------------------------------------

    public void SetDelay(float delay) {
        delayBeforeFade = delay;
    }

    public void SetDuration(float duration) {
        durationOfFade = duration;
    }

    void ShowIDs() {
        //show vanished IDs on game over
        currIDText.color = new Color(currIDText.color.r, currIDText.color.g, currIDText.color.b, 1);
    }

//----------------------------------------------------------------------------------------------------------------------

    void Awake() {
        currIDText = GetComponent<TMP_Text>();
    }

    void Start() {
        StartCoroutine(FadeOut(delayBeforeFade, durationOfFade));
    }

//----------------------------------------------------------------------------------------------------------------------

    //REF: https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
    IEnumerator FadeOut(float delay, float duration) {
        yield return new WaitForSeconds(delay);
        float currentTime = 0.0f;

        while (currentTime < duration) {
            float alpha = Mathf.Lerp(1.0f, 0.0f, currentTime / duration);
            currIDText.color = new Color(currIDText.color.r, currIDText.color.g, currIDText.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
