using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class input : MonoBehaviour
{
    Vector3 vec;

    // Start is called before the first frame update  
    void Start()  
    {  
          
    }  
  
    // Update is called once per frame  
    void Update()  
    {  
        vec = transform.localPosition;  
        vec.y += Input.GetAxis("Jump") * Time.deltaTime * 20;  
        vec.x += Input.GetAxis("Horizontal") * Time.deltaTime * 20;  
        vec.z += Input.GetAxis("Vertical") * Time.deltaTime * 20;  
        transform.localPosition = vec;  
    }  
}