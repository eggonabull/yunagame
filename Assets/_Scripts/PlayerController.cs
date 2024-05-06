using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera _cam;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Canvas _bg_canvas;

    private float _acceleration = 4.0f;
    private float _deceleration = 8.0f;
    private float _maxSpeed = 2.0f;
    private Vector2 _input = Vector2.zero;
    private Vector2 _speed = Vector2.zero;
    private bool isAttacking = false;

    void GatherInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        isAttacking = Input.GetButtonDown("Fire1");
        _input = new Vector2(horizontal, vertical);
    }

    void Update() {
        GatherInput();
    }

    void FixedUpdate() {
        Move();
    }

    void LateUpdate() {
        if (_input.x < 0) {
            _animator.transform.localScale = new Vector3(1, 1, 1);
        } else if (_input.x > 0) {
            _animator.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (_speed.magnitude > 0.15) {
            _animator.SetBool("Walk", true);
        } else {
            _animator.SetBool("Walk", false);
        }
        if (isAttacking) {
            _animator.SetTrigger("Attack");
        } else {
            _animator.ResetTrigger("Attack");    
        }
    }

    void Move() {
        if (_input.x == 0) {
            _speed.x = Mathf.Lerp(_speed.x, 0, _deceleration * Time.fixedDeltaTime);
        } else {
            _speed.x = Mathf.Lerp(_speed.x, _input.x * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }
        if (_input.y == 0) {
            _speed.y = Mathf.Lerp(_speed.y, 0, _deceleration * Time.fixedDeltaTime);
        } else {
            _speed.y = Mathf.Lerp(_speed.y, _input.y * _maxSpeed, _acceleration * Time.fixedDeltaTime);
        }
        _rb.position = _rb.position + _speed;
        
        // The bg exists in the world, move the camera with the player, but clamp
        // the camera to the bg_canvas factoring in the camera's field of view
        // and the aspect ratio of the screen and the bg_canvas's distance
        // from the camera.
        float buffer = 0.1f;
        float backgroundHeight = _bg_canvas.GetComponent<RectTransform>().rect.height;
        float backgroundWidth = _bg_canvas.GetComponent<RectTransform>().rect.width;
        float minX = _bg_canvas.transform.position.x - (backgroundWidth / 2) + (_cam.orthographicSize * _cam.aspect) - buffer;
        float maxX = _bg_canvas.transform.position.x + (backgroundWidth / 2) - (_cam.orthographicSize * _cam.aspect) + buffer;
        float minY = _bg_canvas.transform.position.y - (backgroundHeight / 2) + (_cam.orthographicSize) - buffer;
        float maxY = _bg_canvas.transform.position.y + (backgroundHeight / 2) - (_cam.orthographicSize) + buffer;
        print(minX + " " + maxX + " " + minY + " " + maxY);
        print(_rb.position.x + " " + _rb.position.y);
        print(_cam.transform.position.x + " " + _cam.transform.position.y);

        float clampedX = Mathf.Clamp(_rb.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(_rb.position.y, minY, maxY);
        _cam.transform.position = new Vector3(clampedX, clampedY, _cam.transform.position.z);
        
    }
}
