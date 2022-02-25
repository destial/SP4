using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int currentLevel = 3;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
}
