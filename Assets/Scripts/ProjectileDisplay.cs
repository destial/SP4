using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileDisplay : MonoBehaviour
{
    private Text projText;
    // Start is called before the first frame update
    void Start()
    {
        projText = GetComponent<Text>();
        projText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerThrowing pt = PlayerManager.instance.GetPlayerThrowing();
        GameObject throwable = pt.throwableObject;
        int amount = pt.GetAmount(throwable);
        projText.text = throwable.name + ": " + amount;
    }
}
