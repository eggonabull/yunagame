using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Strategy
{
    Wait,

    Chase,
    Attack,
    Run,
}


public class TreeEnemy : MonoBehaviour
{
    private int health = 3;
    private Strategy strategy = Strategy.Wait;
    [SerializeField] public Transform player;
    [SerializeField] public Rigidbody2D self;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _projectile;

    private float CHASE_UPPER_BOUND = 85;
    private float ATTACK_UPPER_BOUND = 50;
    private float RUN_UPPER_BOUND = 30;
    private float TIME_BETWEEN_ATTACKS = 10f / 12f;
    private float time_since_last_attack = 0;

    private static float CHASE_SPEED = 18;
    private static float RUN_SPEED = CHASE_SPEED * 1.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ticks++;
        // if (ticks % 60 != 0) {
        //     return;
        // }

        if (health <= 0)
        {
            return;
        }

        time_since_last_attack += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer > CHASE_UPPER_BOUND)
        {
            strategy = Strategy.Wait;
            _animator.SetBool("isAttacking", false);
            _animator.SetFloat("speed", 0f);
            return;
        }

        if (distanceToPlayer <= RUN_UPPER_BOUND || (strategy == Strategy.Run && distanceToPlayer <= RUN_UPPER_BOUND + 7))
        {
            _animator.SetBool("isAttacking", false);
            _animator.SetFloat("speed", 1f);
            strategy = Strategy.Run;
            lookAwayFromPlayer();
            self.position = Vector2.MoveTowards(self.position, player.position, -RUN_SPEED * Time.deltaTime);
            return;
        }

        Vector2 target_position = getTargetPosition();
        float distanceToTarget = Vector2.Distance(target_position, self.position);


        if (distanceToTarget > 5)
        {
            strategy = Strategy.Chase;
            lookAtPlayer();
            self.position = Vector2.MoveTowards(self.position, target_position, CHASE_SPEED * Time.deltaTime);
            _animator.SetBool("isAttacking", false);
            _animator.SetFloat("speed", 1f);
        }
        else if (canAttack())
        {
            strategy = Strategy.Attack;
            enterAttack();
        }
        else {
            strategy = Strategy.Wait;
            _animator.SetBool("isAttacking", false);
            _animator.SetFloat("speed", 0f);
        }
    }


    Vector2 getTargetPosition()
    {
        Vector2 target_position = new Vector2();
        if (player.position.x > self.position.x)
        {
            target_position.x = player.position.x - (RUN_UPPER_BOUND + ATTACK_UPPER_BOUND) / 2;
        }
        else
        {
            target_position.x = player.position.x + (RUN_UPPER_BOUND + ATTACK_UPPER_BOUND) / 2;
        }
        target_position.y = player.position.y;
        return target_position;
    }

    void lookAwayFromPlayer()
    {

        if (player.position.x > self.position.x)
        {
            self.transform.localScale = new Vector3(32, 32, 1);
        }
        else
        {
            self.transform.localScale = new Vector3(-32, 32, 1);
        }
    }

    void lookAtPlayer()
    {
        if (player.position.x > self.position.x)
        {
            self.transform.localScale = new Vector3(-32, 32, 1);
        }
        else
        {
            self.transform.localScale = new Vector3(32, 32, 1);
        }
    }

    
    bool canAttack()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        float y_distance = Mathf.Abs(player.position.y - self.position.y);
        return distanceToPlayer <= ATTACK_UPPER_BOUND
        && distanceToPlayer > RUN_UPPER_BOUND
        && y_distance < 5
        && time_since_last_attack >= TIME_BETWEEN_ATTACKS;
    }

    public void GetAttacked()
    {
        if (health <= 0)
        {
            return;
        }
        if (_animator.GetBool("isHit"))
        {
            return;
        }

        health -= 1;
        _animator.SetBool("isHit", true);
    }

    public void HitAnimationEnd()
    {
        _animator.SetBool("isHit", false);
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }

    void enterAttack()
    {
        _animator.SetFloat("speed", 0f);
        _animator.SetBool("isAttacking", true);
    }

    void fireProjectile()
    {
        Vector3 projectile_position = new Vector3(self.position.x, self.position.y - 1, 160); // or 160 for z
        GameObject projectile = Instantiate(_projectile, projectile_position, Quaternion.identity);
        projectile.GetComponent<TreeEnemyProjectile>().ignore_id = self.gameObject.GetInstanceID();
        Destroy(projectile, 5f);
        Vector2 direction = new Vector2(player.position.x - self.position.x, player.position.y - self.position.y);
        direction.Normalize();
        if (direction.x > 0)
        {
            projectile.transform.localScale = new Vector3(-32, 32, 1);
        }
        else
        {
            projectile.transform.localScale = new Vector3(32, 32, 1);
        }
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 64;
        time_since_last_attack = 0;
    }
}
