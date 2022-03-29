using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : Item
{
    public override void Effect(Player player)
    {
        var hp = player.Hp; 
        player.Hp = hp + 10 > 100 ? 100 : hp + 10;
    }
    
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var player = col.GetComponent<Player>();
            Effect(player);
            // Destroy();
        }
        base.OnTriggerEnter2D(col);
    }
}
