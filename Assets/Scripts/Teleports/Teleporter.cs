using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 destination;
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
            StartCoroutine(PlayerManager.instance.GetComponent<PlayerTeleport>().DelayTelport(destination));
            LevelManager.instance.currentLevel--;
        }
    }
}
