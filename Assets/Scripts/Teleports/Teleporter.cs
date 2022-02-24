using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 destination;
    private bool teleported = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            Debug.Log(other.gameObject.name);
            teleported = true;
            LevelManager.instance.currentLevel--;
        }
    }

    private void LateUpdate() {
        if (teleported) {
            PlayerManager.instance.transform.position = destination;
            teleported = false;
        }
    }

}
