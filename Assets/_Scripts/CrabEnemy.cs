using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrabEnemy : MonoBehaviour
{
    private int health = 5;
    private Strategy strategy = Strategy.Wait;
    [SerializeField] public Rigidbody2D player;
    public Rigidbody2D self;
    private Animator _animator;

    private float CHASE_UPPER_BOUND = 85;
    private float ATTACK_UPPER_BOUND = 8;
    private float TIME_BETWEEN_ATTACKS = 2;
    private float time_since_last_attack = 3;

    private int anticipationsNeeded = 0;

    private static float CHASE_SPEED = 26;

    private int ATTACK_OFFSET_DISTANCE = 3;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        self = GetComponent<Rigidbody2D>();
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
            _animator.SetBool("isAnticipating", false);
            _animator.SetFloat("speed", 0f);
            return;
        }

        Vector2 target_position = getTargetPosition();
        float distanceToTarget = Vector2.Distance(target_position, self.position);

        if (time_since_last_attack < TIME_BETWEEN_ATTACKS)
        {
            strategy = Strategy.Run;
            lookAwayFromPlayer();
            self.position = Vector2.MoveTowards(self.position, target_position, -CHASE_SPEED * Time.deltaTime);
            _animator.SetBool("isAnticipating", false);
            _animator.SetFloat("speed", 1f);
        }
        else if (canAttack())
        {
            strategy = Strategy.Attack;
            startAnticipatingAttack();
        }
        else if (distanceToTarget > ATTACK_OFFSET_DISTANCE + 1)
        {
            strategy = Strategy.Chase;
            lookAtPlayer();
            self.position = Vector2.MoveTowards(self.position, target_position, CHASE_SPEED * Time.deltaTime);
            _animator.SetBool("isAnticipating", false);
            _animator.SetFloat("speed", 1f);
        }
        else
        {
            strategy = Strategy.Wait;
            _animator.SetBool("isAnticipating", false);
            _animator.SetFloat("speed", 0f);
        }
    }


    Vector2 getTargetPosition()
    {
        Vector2 target_position = new Vector2();
        if (player.position.x > self.position.x)
        {
            target_position.x = player.position.x - ATTACK_OFFSET_DISTANCE;
        }
        else
        {
            target_position.x = player.position.x + ATTACK_OFFSET_DISTANCE;
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
        && y_distance < 3
        && time_since_last_attack >= TIME_BETWEEN_ATTACKS;
    }

    void startAnticipatingAttack()
    {
        // start the anticipation animation
        if (!_animator.GetBool("isAnticipating"))
        {
            anticipationsNeeded = 2;
            _animator.SetBool("isAnticipating", true);
            _animator.SetFloat("speed", 0f);
        }
    }

    void finishAnticipation()
    {
        anticipationsNeeded -= 1;
        if (anticipationsNeeded == 0)
        {
            enterAttack();
        }
    }

    void enterAttack()
    {
        _animator.SetBool("isAnticipating", false);
        _animator.SetBool("isAttacking", true);
    }


    void fireProjectile()
    {
        Vector3 projectile_position = new Vector3(self.position.x, self.position.y - 1, 160); // or 160 for z
        float xdiff = Mathf.Abs(player.position.x - self.position.x);
        float ydiff = Mathf.Abs(player.position.y - self.position.y);
        if (xdiff < 10 && ydiff < 5)
        {
            Vector2 velocity = (player.position - self.position).normalized * 10000;
            int damage = 29;
            player.GetComponent<PlayerController>().TakeDamage(velocity, damage);
        }
    }

    void finishAttack()
    {
        _animator.SetBool("isAttacking", false);
        time_since_last_attack = 0;
    }

    public void GetAttacked()
    {
        print("CrabEnemy GetAttacked " + _animator.GetBool("isHit"));
        if (health <= 0)
        {
            return;
        }
        if (!_animator.GetBool("isHit"))
        {
            health -= 1;
            _animator.SetBool("isHit", true);
        }
    }

    void getAttacked_Complete()
    {
        print("CrabEnemy getAttacked_Complete " + health);
        if (health <= 0)
        {
            _animator.SetBool("isDead", true);
        }
        _animator.SetBool("isHit", false);

    }

    void die_Complete()
    {
        Destroy(gameObject);
    }

}
