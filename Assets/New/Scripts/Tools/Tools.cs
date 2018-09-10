using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{

    //求相反方向的工具函数
    public static Direction Opposite(Direction rnd)
    {
        if (rnd == Direction.NULL)
        {
            Debug.Log("【Tools】：Opposite方法不应该传入值为Direction.NULL的参数！");
            return Direction.NULL;
        }
        switch (rnd)
        {
            case Direction.LEFT:
                return Direction.RIGHT;
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.UP:
                return Direction.DOWN;
            case Direction.DOWN:
                return Direction.UP;
            default:
                return Direction.NULL;
        }
    }

    //有生命限制的GameObject，在Start里面调用，范例：StartCoroutine(Tools.LimitedLife(Duration, gameObject));
    public static IEnumerator LimitedLife(float lifetime, GameObject obj)
    {
        yield return new WaitForSeconds(lifetime);
        GameObject.Destroy(obj);
    }

    //有生命限制的组件
    public static IEnumerator LimitedLifeComponent(float lifetime, Component component)
    {
        yield return new WaitForSeconds(lifetime);
        GameObject.Destroy(component);
    }

    //绝对Counter，用于各种GameObject的主Controller计算其ID
    private static int COUNTER = 0;
    //绝对Counter的用法
    public static void COUNTIT(GameObject go)
    {
        go.name += " - " + COUNTER;
        COUNTER++;
    }

    //获得随机MobGroupType的方法
    public static MobGroupType GetRandomMobGroupType()
    {
        return (MobGroupType)Random.Range(0, 6);
    }
}

public enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    NULL,
}

public enum MobGroupType
{
    Standard,
    AllSmall,
    AllNormal,
    AllLarge,
    SmallandNormal,
    SmallandLarge,
    NormalandLarge
}

//单条属性的类
public class StatusModifier
{
    private string AttributeKey;
    private float AttributeAddChange;
    private float AttributeMultiplierChange;
    private float AttributeMoreChange;

    public StatusModifier(string key, float addchange, float multipierchange, float morechange)
    {
        AttributeKey = key;
        AttributeAddChange = addchange;
        AttributeMultiplierChange = multipierchange;
        AttributeMoreChange = morechange;
    }

    public void ApplyTo(POEStatus S)
    {
        if (S == null)
        {
            Debug.LogErrorFormat("【{0}】：ApplyTo方法不应该传入值为null的POEStatus参数！", this.GetType());
            return;
        }
        S.S[AttributeKey].AddChange(AttributeAddChange);
        S.S[AttributeKey].MultiplierChange(AttributeMultiplierChange);
        S.S[AttributeKey].MoreChange(AttributeMoreChange);
    }

    public void RemoveFrom(POEStatus S)
    {
        if (S == null)
        {
            Debug.LogErrorFormat("【{0}】：RemoveFrom方法不应该传入值为null的POEStatus参数！", this.GetType());
            return;
        }
        S.S[AttributeKey].AddChange(-AttributeAddChange);
        S.S[AttributeKey].MultiplierChange(-AttributeMultiplierChange);
        S.S[AttributeKey].MoreChange(-AttributeMoreChange);
    }

    public override string ToString()
    {
        string rtn = "";
        if (AttributeAddChange > 0)
        {
            rtn += string.Format("{0}基础值增加{1} \n", AttributeKey, AttributeAddChange.ToString());
        }
        if (AttributeAddChange < 0)
        {
            rtn += string.Format("{0}基础值减少{1} \n", AttributeKey, (-AttributeAddChange).ToString());
        }
        if (AttributeMultiplierChange > 0)
        {
            rtn += string.Format("{0}提高{1}% \n", AttributeKey, (AttributeMultiplierChange * 100f).ToString());
        }
        if (AttributeMultiplierChange < 0)
        {
            rtn += string.Format("{0}降低{1}% \n", AttributeKey, (AttributeMultiplierChange * -100f).ToString());
        }
        if (AttributeMoreChange > 0)
        {
            rtn += string.Format("{0}%更多{1} \n", (AttributeMoreChange * 100f).ToString(), AttributeKey);
        }
        if (AttributeMoreChange < 0)
        {
            rtn += string.Format("{0}%更少{1} \n", (AttributeMoreChange * -100f).ToString(), AttributeKey);
        }
        return rtn;
        //AttributeKey + "," + AttributeAddChange + "," + AttributeMultiplierChange + "," + AttributeMoreChange;
    }
}

//静态类
public static class POEStatics {

    //玩家与玩家组件
    public static GameObject Player;    //玩家唯一实例
    public static PlayerController PlayerController;    //玩家身上的PlayerController组件唯一实例
    public static LevelController PlayerLevelController;    //玩家身上的LevelController组件唯一实例
    public static HPControllerPlayer PlayerHPController;  //玩家身上的HPController组件唯一实例
    public static POEStatus PlayerPOEStatus;    //玩家身上的POEStatus组件唯一实例
    public static MeshRenderer PlayerMeshRenderer;    //玩家身上的MeshRenderer组件唯一实例

    //其他全局唯一组件
    public static RoomManager RoomManager;
    public static CardManager CardManager;

    //玩家属性
    public static int MobLevel = POEStatics.Const.InitLevel; //当前怪物级别
    public static float MobPopulation = POEStatics.Const.InitMobPopulation;   //当前怪物人口
    public static int PlayerCoins; //当前玩家金币

    //其他
    public static bool ISREBORN = false;    //用来表现当前处于重生逻辑中的标记位

    //全局常量
    public static class Const {
        public const int InitLevel = 1; //玩家和怪物通用的初始等级，降序遍历玩家等级表的时候，用Level.Count减掉它即可
        public const float InitMobPopulation = 5f;  //怪物初始等级
        public const string PlayerTag = "Player";
        public const string EnemyTag = "Enemy";
        public const string WallTag = "Wall";
        public readonly static Color PlayerMeshRendererOriginColor = Color.white;
    }
}