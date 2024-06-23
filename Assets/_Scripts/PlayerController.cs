using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera _cam;
    [SerializeField] private Rigidbody2D characterBody;
    [SerializeField] private SuperMap background;

    [SerializeField] private WillHealth willHealth;

    [Header("Footsteps")]
    public List<AudioClip> beachFootsteps;
    public List<AudioClip> grassFootsteps;
    public List<AudioClip> stoneFootsteps;

    [SerializeField] AudioSource footstepAudioSource;

    private float _acceleration = 8.0f;
    private float _deceleration = 12.0f;
    private float _maxSpeed = 50.0f;
    private Vector2 _input = Vector2.zero;
    private Vector2 _speed = Vector2.zero;
    private bool isAttacking = false;

    private int maxHealth = 100;
    private int health = 100;

    float minX;
    float maxX;
    float minY;
    float maxY;

    void Awake()
    {
        // Get the bounds of the background map
        float buffer = 0.1f;
        float backgroundHeight = background.m_Height * background.transform.localScale.y * background.GetComponentInChildren<Grid>().cellSize.y;
        float backgroundWidth = background.m_Width * background.transform.localScale.x * background.GetComponentInChildren<Grid>().cellSize.x;
        float cam_width = _cam.orthographicSize * _cam.aspect;
        _animator.SetBool("Walk", false);
        minX = background.transform.position.x + cam_width + buffer;
        maxX = background.transform.position.x + backgroundWidth - cam_width - buffer;
        minY = background.transform.position.y - backgroundHeight + _cam.orthographicSize + buffer;
        maxY = background.transform.position.y - _cam.orthographicSize - buffer;
        _cam.transform.position = new Vector3(characterBody.position.x, characterBody.position.y, _cam.transform.position.z);
    }

    void GatherInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        isAttacking = Input.GetButtonDown("Fire1");
        _input = new Vector2(horizontal, vertical);
    }

    void Update()
    {
        GatherInput();

        //characterBody.position += _speed * Time.fixedDeltaTime;
        
        //_cam.transform.position = Vector3.MoveTowards(_cam.transform.position, clampedPosition, 1f);
    }

    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        if (health <= 0)
        {
            return;
        }

        if (_input.x < 0)
        {
            _animator.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_input.x > 0)
        {
            _animator.transform.localScale = new Vector3(-1, 1, 1);
        }
        // print("Walk " + _animator.GetBool("Walk"));
        if (_speed.magnitude > 0.15 && !_animator.GetBool("Walk"))
        {
            _animator.SetBool("Walk", true);
        }
        else if (_speed.magnitude <= 0.15 && _animator.GetBool("Walk"))
        {
            _animator.SetBool("Walk", false);
        }
        if (isAttacking)
        {
            _animator.SetTrigger("Attack");

            // get if there are any trees in the attack range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(characterBody.position, 8.0f);
            // print("caharacterBody.position " + characterBody.position);
            // print("colliders " + colliders.Length);
            foreach (Collider2D collider in colliders)
            {
                // print("collider " + collider.gameObject.tag);
                if (collider.gameObject.tag == "Tree")
                {
                    TreeScript tree = collider.gameObject.GetComponent<TreeScript>();
                    tree.GetAttacked();
                }
                if (collider.gameObject.tag == "Glacier")
                {
                    Glacier glacier = collider.gameObject.GetComponent<Glacier>();
                    glacier.GetAttacked();
                }
                if (collider.gameObject.tag == "Ghost")
                {
                    GhostScript ghost = collider.gameObject.GetComponent<GhostScript>();
                    ghost.GetAttacked();
                }
                if (collider.gameObject.tag == "Crab")
                {
                    CrabEnemy crab = collider.gameObject.GetComponent<CrabEnemy>();
                    crab.GetAttacked();
                }
            }
        }
        else
        {
            _animator.ResetTrigger("Attack");
        }
    }

    public void TakeDamage(Vector2 velocity, int damage)
    {
        _speed = _speed + velocity * 2f;
        _animator.SetTrigger("Hit");
        print("TakeDamage " + damage);
        health -= damage;
        willHealth.SetHealth(maxHealth, health);
        if (health <= 0)
        {
            _animator.SetBool("Walk", false);
            _animator.SetBool("Death", true);
        }
    }

    void Move()
    {
        if (health <= 0)
        {
            return;
        }

        if (_input.x == 0)
        {
            _speed.x = Mathf.Lerp(_speed.x, 0, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _speed.x = Mathf.Lerp(_speed.x, _input.x * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }
        if (_input.y == 0)
        {
            _speed.y = Mathf.Lerp(_speed.y, 0, _deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _speed.y = Mathf.Lerp(_speed.y, _input.y * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }


        _speed = Vector2.ClampMagnitude(_speed, _maxSpeed);
        characterBody.velocity = _speed;
        // characterBody.position += _speed * Time.fixedDeltaTime;

        // float clampedX = Mathf.Clamp(characterBody.position.x, minX, maxX);
        // float clampedY = Mathf.Clamp(characterBody.position.y, minY, maxY);
        // Vector3 clampedPosition = new Vector3(clampedX, clampedY, _cam.transform.position.z);
        // _cam.velocity = _speed;
    }

    
    string GetMaterial() {
        var tileMaps = FindObjectsOfType<Tilemap>();
        int i = 0;
        bool any_sand = false;
        bool any_grass = false;
        bool any_stone = false;
        foreach ( var tm in tileMaps )
        {
            Vector3Int cellPosition = tm.WorldToCell(transform.position);
            cellPosition.z = 0; // Z plan of tiles, might vary for you.
            var superTile = tm.GetTile<SuperTile>(cellPosition);
            if (superTile != null)
            {
                if (superTile.name.Contains("Sand"))
                {
                    any_sand = true;
                }
                else if (superTile.name.Contains("Grass"))
                {
                    any_grass = true;
                }
                else if (superTile.name.Contains("Stone"))
                {
                    any_stone = true;
                }
            }
            i++;
        }

        if (any_stone)
        {
            return "Stone";
        }
        else if (any_grass)
        {
            return "Grass";
        }
        else if (any_sand)
        {
            return "Beach";
        }
        return "Unknown";
    }

    void PlayFootstep()
    {
        AudioClip clip = null;
        string material = GetMaterial();
        // print ("Material: " + material);
        if (material == "Beach")
        {
            clip = beachFootsteps[Random.Range(0, beachFootsteps.Count)];
        }
        else if (material == "Grass")
        {
            clip = grassFootsteps[Random.Range(0, grassFootsteps.Count)];
        }
        else if (material == "Stone")
        {
            clip = stoneFootsteps[Random.Range(0, stoneFootsteps.Count)];
        }
        
        if (clip == null)
        {
            return;
        }
        
        footstepAudioSource.clip = clip;
        footstepAudioSource.volume = Random.Range(0.8f, 1f);
        footstepAudioSource.pitch = Random.Range(0.8f, 1.1f);
        footstepAudioSource.Play();
    }
}
