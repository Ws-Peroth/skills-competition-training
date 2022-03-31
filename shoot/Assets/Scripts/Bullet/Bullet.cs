using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float BulletSpeed { get; set; }
    public Vector3 BulletDirection { get; set; }
    public int BulletDamage { get; set; }
    public PoolCode BulletType { get; set; }
    public string TargetTag { get; set; }

    public abstract void FixedUpdate();

    /// <summary>
    ///  Initialize Base Data : float bulletSpeed, Vector3 bulletDirection, int bulletDamage, PoolCode bulletType, string targetTag 
    /// </summary>
    public abstract void InitializeBaseData();
    
    
    protected virtual void OnBecameInvisible() { }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Border"))
        {
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
    }
}
