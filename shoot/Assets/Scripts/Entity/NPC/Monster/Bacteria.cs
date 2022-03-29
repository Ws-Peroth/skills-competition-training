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
        Hp -= damage;
        // print($"[Bacteria] Damaged : Hp = {Hp}");
        if (Hp == 0)
        {
            IsDestroyed = true;
            PoolManager.Instance.DestroyPrefab(gameObject, EntityType);
        }
    }

    protected override void Attack()
    {
        return;
    }

    protected override void OnBecameInvisible()
    {
        if (IsDestroyed)
        {
            // print("Destroyed");
            return;
        }

        GameManager.Instance.Pain += Damage / 2f;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.Instance.Damaged(Damage / 2f, DamageType.Hp);
        }
        base.OnTriggerEnter2D(col);
    }
}
