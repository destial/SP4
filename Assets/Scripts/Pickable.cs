using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    private bool isPickable = true;
    // Start is called before the first frame update
    public bool getPickable()
    {
        return isPickable;
    }

    public void setPickable(bool p)
    {
        isPickable = p;
    }
}
