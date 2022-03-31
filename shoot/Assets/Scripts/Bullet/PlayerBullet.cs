using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private readonly Color _defaultColor = new Color(0, 0.8861785f, 1, 1);
    private readonly Color _enhanceColor = new Color(1, 0.4f, 0.4f, 1); 
    [SerializeField] private SpriteRenderer bulletSpriteRenderer;
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
            col.GetComponent<Entity>().Damaged(BulletDamage);
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
        base.OnTriggerEnter2D(col);
    }

    protected override void InitializeBaseData()
    {
        BulletDirection = Vector3.up;
        BulletType = PoolCode.PlayerBullet;
        TargetTag = "Enemy";
        // Player Input
        // => float bulletSpeed
        // => int bulletDamage
    }

    public void SetBulletColor()
    {
        bulletSpriteRenderer.color = BulletDamage == 1 ? _defaultColor : _enhanceColor;
    }

    private void Move()
    {
        transform.Translate(BulletDirection * BulletSpeed);
    }
}
