using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPControllerEnemy : HPControllerBase
{

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    new public void TakeDamage(POEStatus Source)
    {
        base.TakeDamage(Source);
        if (CurrentHp <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
