using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpHealItem : Item
{
    public float HealPoint { get; set; } = 20;
    public override void Effect(Player player)
    {
        var hp = GameManager.Instance.Hp;
        GameManager.Instance.Hp = hp + HealPoint > 100 ? 100 : hp + HealPoint;
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
