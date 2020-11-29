using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;


public class CPlayerDamage : CCharacterDamage
{
    public override void Damage(ObscuredFloat damage, string hitEffectName = null)
    {
        base.Damage(damage);
    }
}
