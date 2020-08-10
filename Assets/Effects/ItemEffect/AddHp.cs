using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHp : UsableItem.UsageEffect
{
    public int HP = 1;
    public override bool Use(CharacterData user)
    {
        if (user.health == 3)
            return false;
        user.health += HP;
        return true;
    }
}
