using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public int maxHealth = 100;
    int currentHealth;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        SoundManager.instance.playSound(hurtSound);
        currentHealth -= damage;

        //play hurt animation
        anim.SetTrigger("hurt");

        if(currentHealth <= 0)
         {
            Die();
         }
    }

    void Die()
    {
        //die animation
        anim.SetBool("die", true);

        SoundManager.instance.playSound(dieSound);
        //disable enemy
        GetComponent<EnemyPatrol>().OnDisable();
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
