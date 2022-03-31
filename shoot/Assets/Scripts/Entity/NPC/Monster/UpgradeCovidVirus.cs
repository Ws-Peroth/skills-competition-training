using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCovidVirus : Entity
{
private PoolCode _bulletType;
    private readonly (float start, float end) _attackDelay = (1.5f, 2f);
    
    public override void InitializeBaseData()
    {
        // print("Init Cancer");
        Hp = 2500;
        Speed = 0.01f;
        Damage = 20;
        Score = 20000;
        EntityType = PoolCode.UpgradeCovidBoss;
        BulletType = PoolCode.UpgradeCovidBossBullet;
        IsDestroyed = false;
    }
    
    private void Shoot(GameObject bullet, Vector3 position, bool isReflect = false)
    {
        var bulletScript = bullet.GetComponent<UpgradeCovidVirusBullet>();
        bulletScript.InitializeBaseData();
        bulletScript.BulletDamage = Damage;
        bulletScript.BulletSpeed = 0.05f;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
        bulletScript.isReflect = isReflect;
    }
    
    private void OnEnable()
    {
        StartCoroutine(AttackPattern());
    }

    private IEnumerator AttackPattern()
    {
        yield return new WaitForSeconds(0.2f);
        var rotate = 0;
        while (true)
        {
            var patternNumber = Random.Range(0, 3);
            Debug.Log($"Pattern : {patternNumber}");
            switch (patternNumber)
            {
                case 0:
                    for (var i = 0; i < 3; i++)
                    {
                        Pattern1(i * 3f + rotate);
                        yield return new WaitForSeconds(0.3f);
                    }

                    break;
                case 1:
                    for (var i = 0; i < 5; i++)
                    {
                        Pattern2(rotate);
                        yield return new WaitForSeconds(1f);
                    }

                    break;
                case 2:
                    Pattern3();
                    yield return new WaitForSeconds(2);
                    break;
            }
            yield return new WaitForSeconds(2);
            rotate++;
            rotate %= 360;
        }
    }

    private void Pattern1(float rotation)
    {
        for (var i = 0; i < 36; i++)
        {
            var bullet = PoolManager.Instance.CreatPrefab(BulletType);
            Shoot(bullet, transform.position, true);
            bullet.transform.Rotate(new Vector3(0, 0, i * 10 + rotation));
        }
    }
    
    private void Pattern2(float rotation)
    {
        for (var i = 0; i < 36; i++)
        {
            var bullet = PoolManager.Instance.CreatPrefab(BulletType);
            Shoot(bullet, transform.position);
            bullet.transform.Rotate(new Vector3(0, 0, i * 10  + rotation));
        }
    }

    private void Pattern3()
    {
        for (var i = 0; i < 2; i++)
        {
            GameManager.Instance.Spawn(PoolCode.Cancer);
        }
    }
    protected override void Attack()
    {
        return;
    }
    
    void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (transform.position.y > 3.5f)
        {
            transform.Translate(Vector3.down * Speed);
        }
    }

    public override void Damaged(float damage)
    {
        Hp -= damage;
        
        if (Hp <= 0)
        {
            Debug.Log("Ending");
            IsDestroyed = true;
            GameManager.Instance.GetScore(Score);
            PoolManager.Instance.DestroyPrefab(gameObject, EntityType);
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
