using UnityEngine;

public class Health : MonoBehaviour
{
   [SerializeField] public float startingHealth;
    public float currentHealth { get; private set;}
    private Animator anim;
    private bool dead;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;

    [Header ("Components")]
    [SerializeField] Behaviour[] components;
                      

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }
        


    public void takeDamage(float _damage)
    {
        SoundManager.instance.playSound(hurtSound);
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if(currentHealth > 0)
        {
            //player hurt 
            anim.SetTrigger("hurt");
        }
        else 
        {
            if(!dead)
            {            
            anim.SetTrigger("die");
            SoundManager.instance.playSound(dieSound);
  
            foreach (Behaviour component in components)
            {
                component.enabled = false;
            }

            dead = true; 
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

   
}