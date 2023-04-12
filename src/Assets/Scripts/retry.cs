using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class retry : MonoBehaviour {
    
    public GameObject wipe;

    public void Retry() {
        GameObject newWipe = Instantiate(wipe, new Vector3(0, 0, 0), Quaternion.identity);
        newWipe.name = "wipe";
    }
}
