using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public PoolCode ItemType { get; set; }
    public abstract void Effect(Player player);
    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Border"))
        {
            PoolManager.Instance.DestroyPrefab(gameObject, ItemType);
        }
    }
}
