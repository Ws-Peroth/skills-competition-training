using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancerBullet : Bullet
{
    private float _direction;

    private void OnEnable()
    {
        _direction = 0;
    }

    public override void InitializeBaseData()
    {
        BulletDirection = Vector3.down;
        BulletType = PoolCode.CancerBullet;
        TargetTag = "HitBox";
        // Enemy Input
        // => float bulletSpeed
        // => int bulletDamage
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeBaseData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        Move();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(TargetTag))
        {
            col.GetComponentInParent<Player>().Damaged(BulletDamage, DamageType.Hp);
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
        base.OnTriggerEnter2D(col);
    }
    
    private void Move()
    {
        _direction += Time.deltaTime;
        transform.Translate(BulletDirection * BulletSpeed * (1 - _direction));
    }
}
