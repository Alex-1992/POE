using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerEnemy : LevelControllerBase
{

    protected override void Awake()
    {
        S = gameObject.GetComponent<POEStatus>();
        base.Awake();

        List<StatusModifier> temp1 = new List<StatusModifier>()
        {
            new StatusModifier(K.MaxHP, 0, 0.1f, 0),
            new StatusModifier(K.BulletPhysicalDamage, 0, 0.1f, 0),
            new StatusModifier(K.BulletFireDamage, 0, 0.1f, 0),
            new StatusModifier(K.BulletColdDamage, 0, 0.1f, 0),
            new StatusModifier(K.BulletLightinglDamage, 0, 0.1f, 0),
            new StatusModifier(K.MoveSpeed, 0, 0.1f , 0)
        };

        List<StatusModifier> temp2 = new List<StatusModifier>()
        {
            new StatusModifier(K.MaxHP, 0, 0.2f, 0),
            new StatusModifier(K.BulletFireDamage, 0, 0.2f, 0),
            new StatusModifier(K.BulletColdDamage, 0, 0.2f, 0),
            new StatusModifier(K.BulletLightinglDamage, 0, 0.2f, 0),
            new StatusModifier(K.MoveSpeed, 0, 0.1f , 0)
        };

        List<StatusModifier> temp3 = new List<StatusModifier>()
        {
            new StatusModifier(K.MaxHP, 0, 0.3f, 0),
            new StatusModifier(K.BulletFireDamage, 0, 0.3f, 0),
            new StatusModifier(K.BulletColdDamage, 0, 0.3f, 0),
            new StatusModifier(K.BulletLightinglDamage, 0, 0.3f, 0),
            new StatusModifier(K.MoveSpeed, 0, 0.1f , 0)
        };

        Level = new List<LevelData>() {
            new LevelData(){Level = 0, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = null},
            new LevelData(){Level = 1, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = null },
            new LevelData(){Level = 2, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 3, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 4, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 5, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 6, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 7, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 8, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 9, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 10, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp1 },
            new LevelData(){Level = 11, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 12, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 13, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 14, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 15, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 16, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 17, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 18, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 19, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 20, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp2 },
            new LevelData(){Level = 21, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 22, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 23, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 24, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 25, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 26, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 27, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 28, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 29, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 30, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 31, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
            new LevelData(){Level = 32, TotalExpToNextLevel = float.PositiveInfinity , StatusModifiers = temp3 },
        };

        //计算该房间内的怪物应有的等级，并且让怪升到对应的等级
        if (POEStatics.MobLevel > POEStatics.Const.InitLevel)
        {
            LevelUp(POEStatics.MobLevel - POEStatics.Const.InitLevel);
        }
    }

    protected override void LevelUp(int levleup)
    {
        base.LevelUp(levleup);
    }

    public override void ExpGain(float expgain)
    {
        base.ExpGain(expgain);
    }
}
