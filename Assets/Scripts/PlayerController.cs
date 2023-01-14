using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private int maxHealth = 10;
    private int _currentHealth;

    private float _invincibleTime = 2f;
    private float _invincibleTimer;
    private bool _isInvincible;

    private Vector2 _lookDirection = new Vector2(1, 0);

    private Rigidbody2D _rigidbody; //刚体组件

    private Animator _animator;
    private static readonly int LookX = Animator.StringToHash("Look X");
    private static readonly int LookY = Animator.StringToHash("Look Y");
    private static readonly int Speed = Animator.StringToHash("Speed");

    public GameObject bulletPrefab;
    private static readonly int Launch = Animator.StringToHash("Launch");

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = 2;

        _invincibleTimer = 0;
        _isInvincible = true;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //水平移动方向 A:-1 D:1
        float moveY = Input.GetAxisRaw("Vertical"); //垂直移动方向 W:1 S:-1

        Vector2 moveVector = new Vector2(moveX, moveY);
        if (moveVector.x != 0 || moveVector.y != 0)
        {
            _lookDirection = moveVector;
        }

        _animator.SetFloat(LookX, _lookDirection.x);
        _animator.SetFloat(LookY, _lookDirection.y);
        _animator.SetFloat(Speed, moveVector.magnitude);

        Vector2 pos = _rigidbody.position;

        // pos.x += moveX * speed * Time.fixedDeltaTime;
        // pos.y += moveY * speed * Time.fixedDeltaTime;
        pos += moveVector * (speed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(pos);


        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0)
            {
                _isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            _animator.SetTrigger(Launch);
            GameObject bullet = Instantiate(bulletPrefab, _rigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (bulletController != null)
            {
                bulletController.Move(_lookDirection, 300);
            }
        }
    }

    public void UpdateHealth(int health)
    {
        if (health < 0)
        {
            if (_isInvincible)
            {
                return;
            }

            _invincibleTimer = _invincibleTime;
            _isInvincible = true;
        }

        _currentHealth = Mathf.Clamp(_currentHealth + health, 0, maxHealth);
        Debug.Log("HP: " + _currentHealth + "/" + maxHealth);
    }
}