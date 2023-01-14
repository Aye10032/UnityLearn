using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Move(Vector2 moveDirection, float moveForce)
    {
        _rigidbody2D.AddForce(moveDirection * moveForce);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        EnemyController enemyController = col.gameObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            Debug.Log("hit!");
            enemyController.Fixed();
        }
        Destroy(this.gameObject);
    }
}