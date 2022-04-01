using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;


public class Bacteria : Entity
{
    public override void InitializeBaseData()
    {
        // print("Init Bacteria");
        Hp = 5;
        Speed = 0.1f;
        Damage = 2;
        Score = 100;
        EntityType = PoolCode.Bacteria;
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
        if(Hp <= 0) return;
        Hp -= damage;
        
        if (Hp <= 0)
        {
            Killed();
        }
    }

    public override void Killed()
    {
        IsDestroyed = true;
        GameManager.Instance.GetScore(Score);
        PoolManager.Instance.DestroyPrefab(gameObject, EntityType);
    }

    protected override void Attack()
    {
        return;
    }

    protected override void OnBecameInvisible()
    {
        if (IsDestroyed)
        {
            return;
        }

        GameManager.Instance.Pain += Damage / 2f;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Player>().Damaged(Damage / 2f, DamageType.Hp);
        }
        base.OnTriggerEnter2D(col);
    }
}
