using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    Animator _animator;
    int health = 3;
    [SerializeField] public EarthHealth earthHealth;
    [SerializeField] public TreeEnemy treeEnemy;

    public void Start()
    {
        _animator = GetComponent<Animator>();

        earthHealth.IncreaseHealth(1);
    }

    // Start is called before the first frame update
    public void GetAttacked()
    {
        if (health > 0)
        {
            health -= 1;
            _animator.SetInteger("Health", health);

            if (health == 0)
            {
                earthHealth.DecreaseHealth(1);

                // Spawn a tree enemy
                Instantiate(treeEnemy, transform.position, Quaternion.identity);

            }
        }
    }
}
