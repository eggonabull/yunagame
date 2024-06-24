using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    Animator _animator;
    int health = 1;
    [SerializeField] public EarthHealth earthHealth;

    // Start is called before the first frame update
    void Start()
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

            if (health == 0)
            {
                _animator.SetBool("isSpilled", true);
                earthHealth.DecreaseHealth(1);
            }
        }
    }
}
