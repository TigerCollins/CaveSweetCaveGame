using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueBerry : Berry
{
    public override void Activate(PlayerController _player)
    {
        base.Activate(_player);
        _player.glueBerries++;
        Destroy(this.gameObject);
    }
}
