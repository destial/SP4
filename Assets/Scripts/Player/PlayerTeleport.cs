using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    CharacterController characterController;
    
    [SerializeField] GameObject TP1;

    Vector3 TP1Location;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        
        TP1Location = TP1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(DelayTelport(TP1Location));
        }
    }

    void TeleportPlayer(Vector3 TPLocation)
    {
        gameObject.transform.position = TPLocation;
        Debug.Log("Telported to " + TPLocation);
    }

    IEnumerator DelayTelport(Vector3 TPLocation)
    {
        //characterController.disabled = true;

        PlayerMovement.instance.disabled = true;
        yield return null;

        TeleportPlayer(TPLocation);
        yield return null;

        PlayerMovement.instance.disabled = false;
    }
}
