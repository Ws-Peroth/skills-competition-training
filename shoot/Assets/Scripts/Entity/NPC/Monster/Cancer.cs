using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancer : Entity
{
    private PoolCode _bulletType;
    private readonly (float start, float end) _attackDelay = (1.5f, 2f);
    
    public override void InitializeBaseData()
    {
        // print("Init Cancer");
        Hp = 40;
        Speed = 0.04f;
        Damage = 20;
        Score = 1000;
        EntityType = PoolCode.Cancer;
        BulletType = PoolCode.CancerBullet;
        IsDestroyed = false;
    }
    
    private void Shoot(GameObject bullet, Vector3 position)
    {
        var bulletScript = bullet.GetComponent<CancerBullet>();
        bulletScript.InitializeBaseData();
        bulletScript.BulletDamage = Damage;
        bulletScript.BulletSpeed = 0.05f;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
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
            yield return new WaitForSeconds(0.5f);
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
        for (var i = 0; i < 8; i++)
        {
            var bullet = PoolManager.Instance.CreatPrefab(BulletType);
            Shoot(bullet, transform.position);
            bullet.transform.Rotate(new Vector3(0, 0, i * 45));
        }
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
