using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 100f;
    public void TakeDamage(float damage)
    {
        health -= damage;
            
    }

    public float GetHP()
    {
        return health;
    }
}
