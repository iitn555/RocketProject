using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Unit
{
    public override void Respawn()
    {

    }

    private void Awake()
    {
        MaxHp = 10;
        Init();
    }
}
