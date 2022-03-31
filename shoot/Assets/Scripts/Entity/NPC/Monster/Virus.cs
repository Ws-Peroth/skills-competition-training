using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Entity
{
    private PoolCode _bulletType;
    
    private readonly (float start, float end) _attackDelay = (1f, 1.5f);
    public override void InitializeBaseData()
    {
        // print("Init Virus");
        Hp = 25;
        Speed = 0.04f;
        Damage = 10;
        Score = 700;
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
        yield return new WaitForSeconds(0.2f);
        
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(Random.Range(_attackDelay.start, _attackDelay.end));
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
        
        if (Hp <= 0)
        {
            IsDestroyed = true;
            GameManager.Instance.GetScore(Score);
            PoolManager.Instance.DestroyPrefab(gameObject, EntityType);
        }
    }

    protected override void Attack()
    {
        for (var i = 0; i < 5; i++)
        {
            var bullet = PoolManager.Instance.CreatPrefab(BulletType);
            Shoot(bullet, transform.position);
            // i = 0 1 2 3 4
            bullet.transform.Rotate(new Vector3(0, 0, (i - 2) * 10));
        }
    }

    private void Shoot(GameObject bullet, Vector3 position)
    {
        var bulletScript = bullet.GetComponent<VirusBullet>();
        bulletScript.InitializeBaseData();
        bulletScript.BulletDamage = Damage;
        bulletScript.BulletSpeed = 0.1f;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
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