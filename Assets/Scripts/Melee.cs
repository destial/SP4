using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public GameObject melee;
    public bool canAttack = true;
    public float coolDown = 1.0f;



    private AudioSource Sound;
    private void Start()
    {
        Sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            if(canAttack)
            {
                MeleeAttack();
            }
        }
    }


    public void MeleeAttack()
    {
        canAttack = false;
        Animator anim = melee.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        Sound.Play();
        Collider[] hit = Physics.OverlapBox(melee.transform.position, transform.localScale / 2, Quaternion.identity);
        for(int i =0; i < hit.Length; i++)
        {
            if(hit[i].gameObject == melee)
            {
                continue;
            }
            IDamageable damageable = hit[i].GetComponent<IDamageable>();
            damageable?.TakeDamage(5);
            Debug.Log(damageable?.GetHP());
            Debug.Log("Hit: " + hit[i].name);
        }
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(coolDown);
        canAttack = true;
    }
}
