using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

[DisallowMultipleComponent]
[RequireComponent(typeof(POEStatus))]
[RequireComponent(typeof(HPControllerBase))]
public class LevelControllerBase : MonoBehaviour
{
    protected POEStatus S;
    protected List<LevelData> Level;
    protected int CurrentLevel;
    protected float CurrentExp;

    //debug
    private const bool isDebug = false;

    protected virtual void Awake()
    {
        //通用初始化逻辑
        CurrentExp = 0;
        CurrentLevel = POEStatics.Const.InitLevel;
    }

    //获得经验
    public virtual void ExpGain(float expgain)
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
    protected virtual void LevelUp(int levleup)
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