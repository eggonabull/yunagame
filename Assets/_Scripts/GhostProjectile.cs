using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProjectile : MonoBehaviour
{
    public Transform player;
    public int ignore_id;
    private Rigidbody2D rb;


    private int damage = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            Vector2 target = (player.position - transform.position).normalized;
            rb.velocity = rb.velocity + target * Time.deltaTime * 700;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg);
        }
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
            Destroy(gameObject, 0.05f);
        }
    }
}
