using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSettings : MonoBehaviour
{
    [SerializeField] public float zombieSpeed = 5f;
    [SerializeField] public float aggroRadius = 5f;
    [SerializeField] public float attackRange = 1f;
    [SerializeField] public GameObject zombieProjectilePrefab;

    public static GameSettings Instance;
    
    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
}
