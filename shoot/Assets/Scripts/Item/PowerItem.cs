using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItem : Item
{
    public override void Effect(Player player)
    {
        var power = GameManager.Instance.Power;
        GameManager.Instance.Power = power + 1 > 5 ? 5 : power + 1;
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
