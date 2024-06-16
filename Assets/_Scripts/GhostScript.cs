using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GhostScript : MonoBehaviour
{

    [SerializeField] public Transform player;
    
    [SerializeField] private GameObject _projectile;

    private Animator animator;
    private Renderer renderer;
    private Collider2D collider;


    private bool isAppeared = false;

    private float TIME_TO_APPEAR = 2f;
    private float TIME_TO_DISAPPEAR = 1f;
    private float TIME_TO_ATTACK = 1.1f;

    private float CHASE_UPPER_BOUND = 120;

    private float time_since_last_appear = 0;
    private float time_since_last_disappear = 0;
    private float time_since_last_attack = 0;
    private int health = 1;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider2D>();
        renderer.enabled = false;
        collider.enabled = false;
    }

    void Appear()
    {
        renderer.enabled = true;
        collider.enabled = true;
        // Choose a position near the player to appear
        float x = player.position.x + Random.Range(-50, 50);
        float y = player.position.y + Random.Range(-35, 35);
        transform.position = new Vector3(x, y, transform.position.z);
        time_since_last_appear = 0;
        print("Appearing");
        animator.SetTrigger("Appear");
    }

    void Appear_Complete()
    {
        print("Appeared");
        isAppeared = true;
        time_since_last_disappear = 0;
        animator.ResetTrigger("Appear");
    }

    void Disappear()
    {
        print("Disappearing");
        time_since_last_disappear = 0;
        animator.SetTrigger("Disappear");
    }

    void Disappear_Complete()
    {
        isAppeared = false;
        time_since_last_appear = 0;
        renderer.enabled = false;
        collider.enabled = false;
        print("Disappeared");
        animator.ResetTrigger("Disappear");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FireProjectile()
    {
        Vector3 projectile_position = new Vector3(transform.position.x, transform.position.y - 1, 160); // or 160 for z
        GameObject projectile = Instantiate(_projectile, projectile_position, Quaternion.identity);
        GhostProjectile projectileScript = projectile.GetComponent<GhostProjectile>();
        projectileScript.ignore_id = gameObject.GetInstanceID();
        projectileScript.player = player;
        Destroy(projectile, 5f);
        Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);
        direction.Normalize();
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 5;
        time_since_last_attack = 0;
    }

    void Attack()
    {
        // Attack
        if (isAppeared && time_since_last_attack > TIME_TO_ATTACK)
        {
            // make sure we don't immediately disappear after attacking
            time_since_last_disappear -= 1;
            FireProjectile();
            time_since_last_attack = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(25, 25, 1);
        }
        else
        {
            transform.localScale = new Vector3(-25, 25, 1);
        }
        if (!isAppeared)
        {
            time_since_last_appear += Time.deltaTime;
            float distanceToPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceToPlayer < CHASE_UPPER_BOUND)
            {
                if (time_since_last_appear > TIME_TO_APPEAR)
                {
                    Appear();
                }
            }
        }
        else
        {
            time_since_last_attack += Time.deltaTime;
            time_since_last_disappear += Time.deltaTime;
            if (time_since_last_disappear > TIME_TO_DISAPPEAR)
            {
                Disappear();
            }
        }
    }

    public void GetAttacked()
    {
        if (health <= 0)
        {
            return;
        }

        health -= 1;
        animator.SetTrigger("Hit");
    }

    void GetAttacked_Complete()
    {
        animator.ResetTrigger("Hit");
        if (health == 0)
        {
            Disappear();
        }
    }
}
