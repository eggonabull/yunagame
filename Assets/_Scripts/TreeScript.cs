using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public Animator animator;
    int health = 3;

    // Start is called before the first frame update
    public void GetAttacked()
    {
        health -= 1;
        animator.SetInteger("Health", health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
