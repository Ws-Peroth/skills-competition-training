
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Virus : Entity
{
    private PoolCode _bulletType;
    public override void InitializeBaseData()
    {
        // print("Init Virus");
        Hp = 7;
        Speed = 0.04f;
        Damage = 2;
        EntityType = PoolCode.Virus;
        BulletType = PoolCode.VirusBullet;
        IsDestroyed = false;
    }
    
    private void OnEnable()
    {
        StartCoroutine(AttackPattern());
    }

    private IEnumerator AttackPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1));
            Attack();
        }
    }
    
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
        var bullet = PoolManager.Instance.CreatPrefab(BulletType);
        Shoot(bullet, transform.position);
    }

    private static void Shoot(GameObject bullet, Vector3 position)
    {
        var bulletScript = bullet.GetComponent<VirusBullet>();
        bulletScript.BulletDamage = 1;
        bulletScript.BulletSpeed = 0.1f;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
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
