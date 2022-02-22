using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
