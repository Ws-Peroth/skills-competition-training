using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// 백혈구
public class Leukocyte : Entity
{
    private int _itemCallStack;
    public override void InitializeBaseData()
    {
        _itemCallStack = 0;
        Speed = 0.05f;
        EntityType = PoolCode.Leukocyte;
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
        return;
    }

    public override void Killed()
    {
        return;
    }

    protected override void Attack()
    {
        return;
    }
    protected override void OnBecameInvisible()
    {
        if (IsDestroyed)
        {
            // Already Destroy
            return;
        }
        
        // Another Action
    }
    private void DropItem()
    {
        _itemCallStack++;
        if (_itemCallStack > 1)
        {
            return;
        }
        
        IsDestroyed = true;
        var randomIndex = Random.Range((int) PoolCode.PowerUpItem, (int) PoolCode.MaxCount);
        var item = PoolManager.Instance.CreatPrefab((PoolCode) randomIndex);
        item.GetComponent<Item>().ItemType = (PoolCode) randomIndex;
        item.transform.position = transform.position;
        item.SetActive(true);

        // Debug.Log($"[Drop Item] : {(PoolCode) randomIndex} ::: ({_itemCallStack})");
        PoolManager.Instance.DestroyPrefab(gameObject, PoolCode.Leukocyte);
    }
    
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            DropItem();
        }
        else if (col.GetComponent<Bullet>() as PlayerBullet)
        {
            DropItem();
        }

        base.OnTriggerEnter2D(col);
    }
}
