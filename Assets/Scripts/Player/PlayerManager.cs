using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public static GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
