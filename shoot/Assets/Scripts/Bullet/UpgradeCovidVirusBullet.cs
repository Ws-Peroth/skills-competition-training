using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCovidVirusBullet : Bullet
{
    public bool isReflect;
    public override void InitializeBaseData()
    {
        BulletDirection = Vector3.down;
        BulletType = PoolCode.UpgradeCovidBossBullet;
        TargetTag = "HitBox";
        isReflect = false;
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
        if (col.CompareTag("MoveBorder"))
        {
            if (!isReflect)
            {
                isReflect = true;
                transform.Rotate(new Vector3(0, 0, 180));
            }
        }
        base.OnTriggerEnter2D(col);
    }

    private void Move()
    {
        transform.Translate(BulletDirection * BulletSpeed);
    }
}
