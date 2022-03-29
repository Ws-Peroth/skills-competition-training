using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableItem : Item
{
    public override void Effect(Player player)
    {
        player.isUnbreakable = true;
        player.unbreakableTime = 3f;
    }
}
