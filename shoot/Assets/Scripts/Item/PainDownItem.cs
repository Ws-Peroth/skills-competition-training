using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainDownItem : Item
{
    public override void Effect(Player player)
    {
        var pain = GameManager.Instance.Pain;
        GameManager.Instance.Pain = pain - 10 < 0 ? 0 : pain - 10;
    }
    
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Effect(null);
            // Destroy();
        }
        base.OnTriggerEnter2D(col);
    }
}
