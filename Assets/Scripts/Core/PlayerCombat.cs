using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator anim;
    public LayerMask enemyLayers;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    [SerializeField] private AudioClip attackSound;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate; 
            }        
        }
        
    }

    void Attack()
        {
            SoundManager.instance.playSound(attackSound);
            //play attack animation
             anim.SetTrigger("attack");
            //Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, enemyLayers);
            
            //Damage enemies
            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(20);
            }
        }
        
        void OnDrawGizmosSelected()
        {
            if(AttackPoint == null)
                return;

            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
        }
}


