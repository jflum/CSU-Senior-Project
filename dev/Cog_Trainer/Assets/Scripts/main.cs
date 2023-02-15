using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

    public int numTiles = 4; //TODO: need setter and getter once difficulty option is implemented
    private field newField;

    void Awake () {
        //Debug.Log("main Awake");
        newField = GetComponent<field>();
    }
    
    void Start() {
        //Debug.Log("main Start");
        newField.BuildField(numTiles, true);        
    }

    void Update() {
        
    }
}