using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public static GUIController instance;
    public GameObject crosshair;
    public GameObject e;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
