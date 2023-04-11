using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour {
    
    public GameObject wipe;

    public void EndGame() {
        GameObject newWipe = Instantiate(wipe, new Vector3(0, 0, 0), Quaternion.identity);
        newWipe.name = "wipe";
    }
}