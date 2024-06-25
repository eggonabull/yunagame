using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glacier : MonoBehaviour
{
    
    int health = 4;
    [SerializeField] public EarthHealth earthHealth;
    [SerializeField] public GhostScript ghostEnemy;

    public List<AudioClip> takeDamageSounds;
    AudioSource takeDamageAudioSource;
    Animator animator;

    
    // Start is called before the first frame update
    void Start()
    {        
        earthHealth.IncreaseHealth(1);
        animator = GetComponent<Animator>();
        takeDamageAudioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame

    public void GetAttacked()
    {
        if (health > 0)
        {
            int randomIndex = Random.Range(0, takeDamageSounds.Count);
            takeDamageAudioSource.clip = takeDamageSounds[randomIndex];
            takeDamageAudioSource.Play();
        }

        health -= 1;
        animator.SetInteger("health", health);

        if (health == 0)
        {
            earthHealth.DecreaseHealth(1);

            // Spawn a ghost enemy
            Instantiate(ghostEnemy, transform.position, Quaternion.identity);
        }
    }
}
