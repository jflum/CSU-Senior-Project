using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{
    public float timeLeft;
    private Vector3 origScale;
    private Vector3 currScale;
    private float origTime;
    private bool timesUp = false;

    private float CHILD_SCALE = (1.0f / 3.0f);

//----------------------------------------------------------------------------------------------------------------------

    public void ResetTimeLeft() {
        timeLeft = origTime;
        currScale.x = (origScale.x * (timeLeft / origTime));
        this.transform.localScale = currScale;
    }

    //overload to keep initialized scale if called during subsequent difficulty progression 
    public void ResetTimeLeft(float newTime) {
        timeLeft = newTime;
        currScale.x = (origScale.x * (timeLeft / origTime));
        this.transform.localScale = currScale;
    }

    public void SetTime(float seconds) {
        timeLeft = seconds;
        origTime = timeLeft;
    }

    public float GetTimeLeft() {
        return timeLeft;
    }

//----------------------------------------------------------------------------------------------------------------------

    void Awake () {
        origScale = new Vector3(this.transform.localScale.x * CHILD_SCALE, this.transform.localScale.y * CHILD_SCALE,
            this.transform.localScale.z * CHILD_SCALE);
        // Debug.Log("childScale: " + CHILD_SCALE);
        // Debug.Log("origScale: " + origScale);
        currScale = origScale;
        origTime  = timeLeft;
    }

    void Update() {     
        if (timeLeft > 0) {         
            timeLeft -= Time.deltaTime;     
            //Debug.Log("time remaning: " + timeLeft);
            currScale.x = (origScale.x * (timeLeft / origTime));
            this.transform.localScale = currScale;
        }

        if (timeLeft <= 0 && !timesUp) {         
            //Debug.Log("time's up");
            timeLeft = 0.0f;
            this.transform.localScale = new Vector3(0, currScale.y, currScale.z);
            timesUp = true;
            GameObject.Find("driver").GetComponent<main>().SetGameOverState(true);
        } 
    }

    void OnEnable() {
        timesUp = false;
    }
}