using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == PlayerManager.instance.gameObject) {
            SceneManager.LoadScene(3);
        }
    }
}
