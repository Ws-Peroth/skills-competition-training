using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Entity : MonoBehaviour
{
    [field:SerializeField] public bool IsDestroyed { get; set; }
    [field:SerializeField] public float Hp { get; set; }
    [field:SerializeField] protected float Speed{ get; set; }

    [field: SerializeField] protected int Damage { get; set; }
    
    [field: SerializeField] protected PoolCode EntityType { get; set; }
    protected abstract void Move();
    public abstract void Damaged(float damage);
    
    // public abstract void MeleeDamaged(bool damage);
    protected abstract void Attack();
    
    /// <summary>
    ///  Initialize Base Data : int Hp, float Speed, int Damage, PoolCode EntityType
    /// </summary>
    public abstract void InitializeBaseData();
    protected abstract void OnBecameInvisible();
    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Border"))
        {
            PoolManager.Instance.DestroyPrefab(gameObject, EntityType);
        }
    }
}
