using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemyProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private int damage = 9;

    // Ignore the collision with the object that has this id
    // This is set by the TreeEnemy script that spawns the projectile
    // So is a destructor of the projectile
    public int ignore_id;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collision " + ignore_id + " " + collision.gameObject.GetInstanceID() + " " + collision.gameObject.tag + " " + collision.gameObject.name);
        if (collision.gameObject.GetInstanceID() == ignore_id)
        {
            return;
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(rb.velocity, damage);
        }
        Destroy(gameObject, 0.05f);
        
    }

}
