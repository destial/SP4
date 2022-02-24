using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    void TeleportPlayer(Vector3 TPLocation)
    {
        gameObject.transform.position = TPLocation;
        Debug.Log("Telported to " + TPLocation);
    }

    public IEnumerator DelayTelport(Vector3 TPLocation)
    {
        //characterController.disabled = true;

        PlayerMovement.instance.disabled = true;
        yield return null;

        TeleportPlayer(TPLocation);
        yield return null;

        PlayerMovement.instance.disabled = false;
    }
}
