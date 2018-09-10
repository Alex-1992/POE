using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

[DisallowMultipleComponent]
[RequireComponent(typeof(POEStatus))]
[RequireComponent(typeof(HPControllerBase))]
public class LevelController : MonoBehaviour
{
    private POEStatus S;
    private List<LevelData> Level;
    private int CurrentLevel;
    private float CurrentExp;

    //debug
    private const bool isDebug = false;

    private void Awake()
    {
        //初始化Statics
        if (POEStatics.PlayerLevelController == null && gameObject.CompareTag(POEStatics.Const.PlayerTag)) POEStatics.PlayerLevelController = this;

        //通用初始化逻辑
        S = gameObject.GetComponent<POEStatus>();
        CurrentExp = 0;
        CurrentLevel = POEStatics.Const.InitLevel;

        //玩家用初始化逻辑
        if (gameObject.CompareTag(POEStatics.Const.PlayerTag))
        {
            InitLevelDataAsPlayer();
        }

        //怪物用初始化逻辑
        if (gameObject.CompareTag(POEStatics.Const.EnemyTag))
        {
            InitLevelDataAsEnemy();
            //计算该房间内的怪物应有的等级，并且让怪升到对应的等级
            if (POEStatics.MobLevel > POEStatics.Const.InitLevel)
            {
                LevelUp(POEStatics.MobLevel - POEStatics.Const.InitLevel);
            }
        }
    }

    private void InitLevelDataAsPlayer()
    {
        Level = new List<LevelData>()
        {
            new LevelData(){Level = 0, TotalExpToNextLevel = 0 , StatusModifiers = null },  //从0开始这样升序遍历的时候就不用再麻烦的+1 -1什么的了，Level的Index能够和CurrentLevel对的上（当InitialLevel == 1的时候）
            new LevelData(){Level = 1, TotalExpToNextLevel = 0 , StatusModifiers = null },
            new LevelData(){Level = 2, TotalExpToNextLevel = 50 , StatusModifiers = null },
            new LevelData(){Level = 3, TotalExpToNextLevel = 100 , StatusModifiers = null },
            new LevelData(){Level = 4, TotalExpToNextLevel = 200 , StatusModifiers = null },
            new LevelData(){Level = 5, TotalExpToNextLevel = 300 , StatusModifiers = null },
            new LevelData(){Level = 6, TotalExpToNextLevel = 500 , StatusModifiers = null },
            new LevelData(){Level = 7, TotalExpToNextLevel = 700 , StatusModifiers = null },
            new LevelData(){Level = 8, TotalExpToNextLevel = 1000 , StatusModifiers = null },
            new LevelData(){Level = 9, TotalExpToNextLevel = 1300 , StatusModifiers = null },
            new LevelData(){Level = 10, TotalExpToNextLevel = 1700 , StatusModifiers = null },
            new LevelData(){Level = 11, TotalExpToNextLevel = 2100 , StatusModifiers = null },
            new LevelData(){Level = 12, TotalExpToNextLevel = 2600 , StatusModifiers = null },
            new LevelData(){Level = 13, TotalExpToNextLevel = 3100 , StatusModifiers = null },
            new LevelData(){Level = 14, TotalExpToNextLevel = 3700 , StatusModifiers = null },
            new LevelData(){Level = 15, TotalExpToNextLevel = 4300 , StatusModifiers = null },
            new LevelData(){Level = 16, TotalExpToNextLevel = 5000 , StatusModifiers = null },
            new LevelData(){Level = 17, TotalExpToNextLevel = 6000 , StatusModifiers = null },
            new LevelData(){Level = 18, TotalExpToNextLevel = 7000 , StatusModifiers = null },
            new LevelData(){Level = 19, TotalExpToNextLevel = 8000 , StatusModifiers = null },
            new LevelData(){Level = 20, TotalExpToNextLevel = 9000 , StatusModifiers = null },
            new LevelData(){Level = 21, TotalExpToNextLevel = 10000 , StatusModifiers = null },
            new LevelData(){Level = 22, TotalExpToNextLevel = 11000 , StatusModifiers = null },
            new LevelData(){Level = 23, TotalExpToNextLevel = 12000 , StatusModifiers = null },
            new LevelData(){Level = 24, TotalExpToNextLevel = 13000 , StatusModifiers = null },
            new LevelData(){Level = 25, TotalExpToNextLevel = 14000 , StatusModifiers = null },
            new LevelData(){Level = 26, TotalExpToNextLevel = 15000 , StatusModifiers = null },
            new LevelData(){Level = 27, TotalExpToNextLevel = 16000 , StatusModifiers = null },
            new LevelData(){Level = 28, TotalExpToNextLevel = 17000 , StatusModifiers = null },
            new LevelData(){Level = 29, TotalExpToNextLevel = 18000 , StatusModifiers = null },
            new LevelData(){Level = 30, TotalExpToNextLevel = 19000 , StatusModifiers = null },
            new LevelData(){Level = 31, TotalExpToNextLevel = 20000 , StatusModifiers = null },
            new LevelData(){Level = 32, TotalExpToNextLevel = 21000 , StatusModifiers = null },
        };
    }

    private void InitLevelDataAsEnemy()
    {
        List<StatusModifier> temp1 = new List<StatusModifier>() {
            new StatusModifier(K.MaxHP, 0, 0.1f, 0),
            new StatusModifier(K.BulletPhysicalDamage, 0, 0.1f, 0),
            new StatusModifier(K.BulletFireDamage, 0, 0.1f, 0),
            new StatusModifier(K.BulletColdDamage, 0, 0.1f, 0),
            new StatusModifier(K.BulletLightinglDamage, 0, 0.1f, 0),
            new StatusModifier(K.MoveSpeed, 0, 0.1f , 0)
        };
        List<StatusModifier> temp2 = new List<StatusModifier>() {
            new StatusModifier(K.MaxHP, 0, 0.2f, 0),
            new StatusModifier(K.BulletFireDamage, 0, 0.2f, 0),
            new StatusModifier(K.BulletColdDamage, 0, 0.2f, 0),
            new StatusModifier(K.BulletLightinglDamage, 0, 0.2f, 0),
            new StatusModifier(K.MoveSpeed, 0, 0.1f , 0)
        };
        List<StatusModifier> temp3 = new List<StatusModifier>() {
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
    }

    //获得经验
    public void ExpGain(float expgain)
    {
        if (expgain <= 0)
        {
            Debug.LogError("【LevelController】：不应该给ExpGain方法传入<=0的参数！");
            return;
        }
        CurrentExp += expgain;
        if (CurrentExp >= Level[Level.Count - POEStatics.Const.InitLevel].TotalExpToNextLevel)
        {
            CurrentExp = Level[Level.Count - POEStatics.Const.InitLevel].TotalExpToNextLevel;
            if (isDebug) Debug.LogFormat("【{0} - {1}】：经验值已达上限！当前经验值：{2}，升到下一级需要：{3}", this.GetType().ToString(), gameObject.name, CurrentExp.ToString(), GetExpToNextLevel());
            return;
        }
        if (isDebug) Debug.LogFormat("【{0} - {1}】：获得了{2}经验，当前经验：{3}，升到下一级需要：{4}", this.GetType(), gameObject.name, expgain, CurrentExp, GetExpToNextLevel());
        for (int i = Level.Count - POEStatics.Const.InitLevel; i > CurrentLevel; i--)    //从最高级往回遍历，从而使得该逻辑支持跳级
        {
            if (CurrentExp >= Level[i].TotalExpToNextLevel)
            {
                LevelUp(i - CurrentLevel);
            }
        }
    }

    //升级
    private void LevelUp(int levleup)
    {
        if (levleup <= 0)
        {
            Debug.LogErrorFormat("【{0}】：不应该给LevelUp方法传入<=0的参数！", this.GetType());
            return;
        }
        for (int i = 0; i < levleup; i++)
        {
            CurrentLevel++;
            if (isDebug) Debug.LogFormat("【{0} - {1}】：升级了，当前等级：{2}，当前经验值：{3}，升到下一级需要：{4}", this.GetType(), gameObject.name, CurrentLevel, CurrentExp, GetExpToNextLevel());
            if (Level[CurrentLevel].StatusModifiers != null)
            {
                foreach (var item in Level[CurrentLevel].StatusModifiers)
                {
                    item.ApplyTo(S);
                    if (isDebug) Debug.LogFormat("【{0}】：{1}属性提升了：{2}", this.GetType(), gameObject.name, item.ToString());
                }
            }
        }
        POEStatics.Player.SendMessage("SetHPbyPercent", 1f, SendMessageOptions.RequireReceiver);  //玩家升级后要加满血，怪的话无所谓
    }

    public void ResetPlayer()
    {
        CurrentExp = 0;
        CurrentLevel = POEStatics.Const.InitLevel;
        POEStatics.PlayerMeshRenderer.material.color = POEStatics.Const.PlayerMeshRendererOriginColor;
    }

    public float GetCurrentExp()
    {
        return CurrentExp;
    }

    public float GetExpToThisLevel()
    {
        return Level[CurrentLevel].TotalExpToNextLevel;
    }

    public float GetExpToNextLevel()
    {
        return Level[CurrentLevel + 1].TotalExpToNextLevel;
    }

    public int GetCurrenLevel()
    {
        return CurrentLevel;
    }
}


//等级表的表头
public class LevelData
{
    public int Level;
    public float TotalExpToNextLevel;
    public List<StatusModifier> StatusModifiers = new List<StatusModifier>();
}