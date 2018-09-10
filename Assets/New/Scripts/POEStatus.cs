using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class K
{
    public const string MaxHP = "MaxHP";
    public const string HPRegeneration = "HPRegeneration";
    public const string HPRegenerationMultiplierWhileMoving = "HPRegenerationMultiplierWhileMoving";
    public const string HPRegenerationMultiplierWhileNotMoving = "HPRegenerationMultiplierWhileNotMoving";
    public const string MaxMP = "MaxMP";
    public const string MPRegeneration = "MPRegeneration";
    public const string MPRegenerationMultiplierWhileMoving = "MPRegenerationMultiplierWhileMoving";
    public const string MPRegenerationMultiplierWhileNotMoving = "MPRegenerationMultiplierWhileNotMoving";
    public const string MoveSpeed = "MoveSpeed";
    public const string AttackSpeed = "AttackSpeed";
    public const string BulletCount = "BulletCount";
    public const string BulletSpeed = "BulletSpeed";
    public const string BulletLifeTime = "BulletLifeTime";
    public const string BulletArc = "BulletArc";
    public const string BulletPhysicalDamage = "BulletPhysicalDamage";
    public const string BulletFireDamage = "BulletFireDamage";
    public const string BulletColdDamage = "BulletColdDamage";
    public const string BulletLightinglDamage = "BulletLightinglDamage";
    public const string BulletDamageMultiplier = "BulletDamageMultiplier";
    public const string GlobalDamageMultiplier = "GlobalDamageMultiplier";
    public const string BulletCriticalChance = "BulletCriticalChange";
    public const string BulletCriticalDamageMultiplier = "BulletCriticalDamageMultiplier";
    public const string BulletDamageCount = "BulletDamageCount";
    public const string BulletMPCost = "BulletMPCost";
    public const string ExpGainOnKill = "ExpGainOnKill";
    public const string PlayerLevelUpCardCount = "PlayerLevelUpCardCount";
}

[DisallowMultipleComponent]
public class POEStatus : MonoBehaviour
{
    public Dictionary<string, StatusClass> S = new Dictionary<string, StatusClass>();

    //单位基础属性
    public float MaxHP;
    public float HPRegeneration;
    public float HPRegenerationMultiplierWhileMoving;
    public float HPRegenerationMultiplierWhileNotMoving;
    public float MaxMP;
    public float MPRegeneration;
    public float MPRegenerationMultiplierWhileMoving;
    public float MPRegenerationMultiplierWhileNotMoving;
    public float MoveSpeed;
    public float BulletPhysicalDamage;
    public float BulletFireDamage;
    public float BulletColdDamage;
    public float BulletLightinglDamage;
    public float BulletDamageMultiplier;
    public float GlobalDamageMultiplier;

    //player属性
    public float AttackSpeed;
    public int BulletCount;
    public float BulletSpeed;
    public float BulletLifeTime;
    public float BulletArc;
    public float BulletCriticalChance;
    public float BulletCriticalDamageMultiplier;
    public int BulletDamageCount;
    public float BulletMPCost;
    public int PlayerLevelUpCardCount;

    //怪物属性
    public float ExpGainOnKill;

    protected void Awake()
    {
        //基础属性
        S.Add(K.MaxHP, new StatusClass(MaxHP, false));
        S.Add(K.HPRegeneration, new StatusClass(HPRegeneration, true));
        S.Add(K.HPRegenerationMultiplierWhileMoving, new StatusClass(HPRegenerationMultiplierWhileMoving, true));
        S.Add(K.HPRegenerationMultiplierWhileNotMoving, new StatusClass(HPRegenerationMultiplierWhileNotMoving, true));
        S.Add(K.MaxMP, new StatusClass(MaxMP, false));
        S.Add(K.MPRegeneration, new StatusClass(MPRegeneration, true));
        S.Add(K.MPRegenerationMultiplierWhileMoving, new StatusClass(MPRegenerationMultiplierWhileMoving, true));
        S.Add(K.MPRegenerationMultiplierWhileNotMoving, new StatusClass(MPRegenerationMultiplierWhileNotMoving, true));
        S.Add(K.MoveSpeed, new StatusClass(MoveSpeed, false));
        S.Add(K.BulletPhysicalDamage, new StatusClass(BulletPhysicalDamage, false));
        S.Add(K.BulletFireDamage, new StatusClass(BulletFireDamage, false));
        S.Add(K.BulletColdDamage, new StatusClass(BulletColdDamage, false));
        S.Add(K.BulletLightinglDamage, new StatusClass(BulletLightinglDamage, false));
        S.Add(K.BulletDamageMultiplier, new StatusClass(BulletDamageMultiplier, false));
        S.Add(K.GlobalDamageMultiplier, new StatusClass(GlobalDamageMultiplier, false));
        
        //玩家属性
        S.Add(K.AttackSpeed, new StatusClass(AttackSpeed, false));
        S.Add(K.BulletCount, new StatusClass(BulletCount, false));
        S.Add(K.BulletSpeed, new StatusClass(BulletSpeed, false));
        S.Add(K.BulletLifeTime, new StatusClass(BulletLifeTime, false));
        S.Add(K.BulletArc, new StatusClass(BulletArc, false));
        S.Add(K.BulletCriticalDamageMultiplier, new StatusClass(BulletCriticalDamageMultiplier, false));
        S.Add(K.BulletDamageCount, new StatusClass(BulletDamageCount, false));
        S.Add(K.BulletMPCost, new StatusClass(BulletMPCost, true));
        S.Add(K.PlayerLevelUpCardCount, new StatusClass(PlayerLevelUpCardCount, true));
        
        //怪物属性
        S.Add(K.ExpGainOnKill, new StatusClass(ExpGainOnKill, false));
    }

    private void Start()
    {
        if (gameObject.CompareTag(POEStatics.Const.PlayerTag) && POEStatics.PlayerPOEStatus == null) POEStatics.PlayerPOEStatus = this;
    }
}

public class StatusClass
{
    private float m_origin;
    private int m_originint;
    private float m_add = 0;
    private float m_multiplier = 0;
    private float m_more = 0;
    private float m_final;
    private int m_finalint;

    private readonly bool isInt = false;
    private readonly bool CanBeNegative = false;

    public StatusClass(float m_origin, bool canbenegative)
    {
        this.m_origin = m_origin;
        this.CanBeNegative = canbenegative;
        this.isInt = false;
    }
    public StatusClass(int m_origin, bool canbenegative)
    {
        this.m_originint = m_origin;
        this.CanBeNegative = canbenegative;
        this.isInt = true;
    }

    public void AddChange(float value)
    {
        m_add += value;
    }
    public void MultiplierChange(float value)
    {
        m_multiplier += value;
    }
    public void MoreChange(float value)
    {
        m_more += value;
    }

    public void Update()
    {
        if (isInt)
        {
            m_finalint = (int)(((float)m_originint + m_add) * (1f + m_multiplier) * (1f + m_more));
        }
        else
        {
            m_final = (m_origin + m_add) * (1f + m_multiplier) * (1f + m_more);
        }
    }

    public float Get()
    {
        Update();
        return GetWithoutUpdate();
    }
    public float GetWithoutUpdate()
    {
        if (isInt)
        {
            Debug.LogErrorFormat("【{0}】：你不应该用这个方法获得返回值，应该用GetInt代替！", this.GetType());
        }
        if (CanBeNegative == false && m_final < 0)
        {
            return 0;
        }
        return m_final;
    }
    public int GetInt()
    {
        Update();
        return GetIntWithoutUpdate();
    }
    public int GetIntWithoutUpdate()
    {
        if (!isInt)
        {
            Debug.LogErrorFormat("【{0}】：你不应该用这个方法获得返回值，应该用Get代替！", this.GetType());
        }
        if (CanBeNegative == false && m_finalint < 0)
        {
            return 0;
        }
        return m_finalint;
    }
}
