using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : Item
{
    public override void Effect(Player player)
    {
        GameManager.Instance.GetScore(1500);
    }
    
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<Player>();
            Effect(player);
            PoolManager.Instance.DestroyPrefab(gameObject, ItemType);
        }
        base.OnTriggerEnter2D(col);
    }
}
