using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public GameObject melee;
    public bool canAttack = true;
    public float coolDown = 1.0f;

    private BoxCollider meleeCollider;
    private float m_thickness = 0.025f;
    private AudioSource Sound;
    private Camera playerView;
    private Vector3 camForward;
    private float damageMultiplier = 1.0f; // For backstabbing
    private void Start()
    {
        Sound = melee.GetComponent<AudioSource>();
        meleeCollider = melee.GetComponent<BoxCollider>();
        playerView = melee.GetComponentInParent<Camera>();
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
        
        // Box collider
        Vector3 _scaledSize = new Vector3(
            meleeCollider.size.x * melee.transform.lossyScale.x,
            meleeCollider.size.y * melee.transform.lossyScale.y,
            meleeCollider.size.z * melee.transform.lossyScale.z);
        float _distance = _scaledSize.y - m_thickness;
        Vector3 _direction = melee.transform.up;
        Vector3 _center = melee.transform.TransformPoint(meleeCollider.center);
        Vector3 _start = _center - _direction * (_distance * 0.5f);
        Vector3 _halfExtends = new Vector3(_scaledSize.x, m_thickness, _scaledSize.z) * 0.5f;
        Quaternion _orientation = melee.transform.rotation;

        Vector3 EnemyView;
        Vector3 playerToEnemyView;
        Collider[] hit = Physics.OverlapBox(_start, _halfExtends, _orientation);
        // Checks all that has collided
        for (int i = 0; i < hit.Length; i++)
        {
            damageMultiplier = 1.0f;
            if (hit[i].gameObject == melee || hit[i].gameObject.name == "Player") // If its the player/melee weapon continue;
            {
                continue;
            }
            IDamageable damageable = null;
            
            if (hit[i].gameObject.GetComponent<IDamageable>() != null)
            {
                damageable = hit[i].gameObject.GetComponent<IDamageable>();
            }
            else if (hit[i].gameObject.GetComponentInChildren<IDamageable>() != null)
            {
                damageable = hit[i].GetComponentInChildren<IDamageable>();
            }
            else if (hit[i].gameObject.GetComponentInParent<IDamageable>() != null)
            {
                damageable = hit[i].gameObject.GetComponentInParent<IDamageable>();
            }
            
            //Backstabbing code
            EnemyView = hit[i].gameObject.transform.forward; //  Enemy's view direction
            playerToEnemyView = (playerView.transform.position - hit[i].gameObject.transform.position).normalized; // Player to enemy view direction
            if (Vector3.Dot(EnemyView, playerToEnemyView) < -0.5f)// Check for backstab
                damageMultiplier = 100.0f;


            damageable?.TakeDamage(5 * damageMultiplier);
            Debug.Log(damageable?.GetHP());
            Debug.Log("Hit: " + hit[i].gameObject.name);
        }
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(coolDown);
        canAttack = true;
    }

}
