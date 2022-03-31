using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainDownItem : Item
{
    public float PainDown { get; set; } = 30;
    public override void Effect(Player player)
    {
        var pain = GameManager.Instance.Pain;
        GameManager.Instance.Pain = pain - PainDown < 0 ? 0 : pain - PainDown;
    }
    
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Effect(null);
            PoolManager.Instance.DestroyPrefab(gameObject, ItemType);
        }
        base.OnTriggerEnter2D(col);
    }
}
