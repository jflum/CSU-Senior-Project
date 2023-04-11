using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class flash : MonoBehaviour {

    private float delayBeforeFlash = 0.0f;
    private float durationOfFlash  = 1.0f;
    private TMP_InputField currInputField;
    private Coroutine flashing;

    public void SetDelay(float delay) {
        delayBeforeFlash = delay;
    }

    public void SetDuration(float duration) {
        durationOfFlash = duration;
    }

    void Awake() {
        currInputField = GetComponent<TMP_InputField>();
    }

    //REF: https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
    IEnumerator Flash(float delay, float duration, Color flashColor) {
        yield return new WaitForSeconds(delay);

        Color origColor = Color.white;
        flashColor = Color.Lerp(origColor, flashColor, 0.6f); //color mixing for less stark color flash
        currInputField.image.color = flashColor;
        float currentTime = 0.0f;

        while (currentTime < duration) {
            Color transition = Color.Lerp(flashColor, origColor, currentTime / duration);
            currInputField.image.color = transition;
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public void FlashInput(Color color) {
        if (flashing != null ) StopCoroutine(flashing);
        flashing = StartCoroutine(Flash(delayBeforeFlash, durationOfFlash, color));
    }
}