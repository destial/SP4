using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // Entity takes damage
    public void TakeDamage(float damage);
    // Gets the hp
    public float GetHP();
}
