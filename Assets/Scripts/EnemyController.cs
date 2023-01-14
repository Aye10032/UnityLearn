using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;

    public bool isVertical;

    private Vector2 _moveDirection;

    public float changeDirectionTime = 4f;
    private float _changeDirectionTimer;

    private bool _isFixed = false;

    public ParticleSystem brokenEffect;
    
    private Rigidbody2D _rigidbody2D;

    private Animator _animator;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int Fix = Animator.StringToHash("fix");

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();

        _moveDirection = isVertical ? Vector2.up : Vector2.right;

        _changeDirectionTimer = changeDirectionTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFixed)
        {
            return;
        }
        
        _changeDirectionTimer -= Time.fixedDeltaTime;
        if (_changeDirectionTimer < 0)
        {
            _moveDirection *= -1;
            _changeDirectionTimer = changeDirectionTime;
        }

        Vector2 pos = _rigidbody2D.position;
        pos.x += _moveDirection.x * speed * Time.fixedDeltaTime;
        pos.y += _moveDirection.y * speed * Time.fixedDeltaTime;

        _rigidbody2D.MovePosition(pos);

        _animator.SetFloat(MoveX, _moveDirection.x);
        _animator.SetFloat(MoveY, _moveDirection.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerController controller = col.gameObject.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.UpdateHealth(-1);
        }
    }
    
    public void Fixed()
    {
        _rigidbody2D.simulated = false;
        _isFixed = true;
        _animator.SetTrigger(Fix);
        if (brokenEffect.isPlaying)
        {
            brokenEffect.Stop();
        }
    }
}