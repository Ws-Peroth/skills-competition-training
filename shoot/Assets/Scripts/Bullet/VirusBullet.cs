using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBullet : Bullet
{
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
            GameManager.Instance.Damaged(BulletDamage, DamageType.Hp);
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
        base.OnTriggerEnter2D(col);
    }

    protected override void InitializeBaseData()
    {
        BulletDirection = Vector3.down;
        BulletType = PoolCode.VirusBullet;
        TargetTag = "Player";
        // Enemy Input
        // => float bulletSpeed
        // => int bulletDamage
    }

    private void Move()
    {
        transform.Translate(BulletDirection * BulletSpeed);
    }
}
