using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적혈구
public class Erythrocyte : Entity
{
    private int _damageStack;
    
    public override void InitializeBaseData()
    {
        _damageStack = 0;
        Speed = 0.05f;
        Damage = 10;
        EntityType = PoolCode.Erythrocyte;
        IsDestroyed = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        transform.Translate(Vector3.down * Speed);
    }

    public override void Damaged(float damage)
    {
        return;
    }

    protected override void Attack()
    {
        return;
    }

    protected override void OnBecameInvisible()
    {
        if (IsDestroyed)
        {
            // Already Destroy
            return;
        }
        
        // Another Action
    }

    private void GetPain()
    {
        _damageStack++;
        if (_damageStack > 1)
        {
            return;
        }
        
        IsDestroyed = true;
        GameManager.Instance.Damaged(Damage, DamageType.Pain);
        
        PoolManager.Instance.DestroyPrefab(gameObject, EntityType);
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GetPain();
        }

        if (col.CompareTag("Enemy"))
        {
            GetPain();
        }

        if (col.CompareTag("Bullet"))
        {
            GetPain();
        }

        base.OnTriggerEnter2D(col);
    }
}
