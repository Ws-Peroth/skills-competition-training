using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidVirusBullet : Bullet
{
    protected override void InitializeBaseData()
    {
        BulletDirection = Vector3.down;
        BulletType = PoolCode.CovidBossBullet;
        TargetTag = "Player";
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
            col.GetComponent<Player>().Damaged(BulletDamage, DamageType.Hp);
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
        base.OnTriggerEnter2D(col);
    }

    private void Move()
    {
        transform.Translate(BulletDirection * BulletSpeed);
    }
}
