using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainsBerry : Berry
{

    public float StrengthGain;
    public float GainTime;

    public override void Activate(PlayerController _player)
    {
        base.Activate(_player);
        _player.BoostStrength(StrengthGain, GainTime);
        Destroy(this.gameObject);
    }
}
