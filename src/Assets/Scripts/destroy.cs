using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour {
    
    void OnDisable() {
        Destroy(this.gameObject);
    }
}
