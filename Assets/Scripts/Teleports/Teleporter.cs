using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector3 destination;

    private void OnTriggerEnter(Collider other) {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null) {
            StartCoroutine(PlayerManager.instance.GetComponent<PlayerTeleport>().DelayTelport(destination));
            LevelManager.instance.currentLevel--;
        }
    }
}
