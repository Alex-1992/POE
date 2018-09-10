using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(POEStatus))]
public abstract class HPControllerBase : MonoBehaviour
{
    //protected
    protected float CurrentHp;
    protected float CurrentMp;
    protected POEStatus S;

    //debug
    private const bool isDebug = true;

    protected void Start()
    {
        S = gameObject.GetComponent<POEStatus>();
        SetHPbyPercent(1f);
        SetMPbyPercent(1f);
    }

    public float GetCurrentHP()
    {
        return CurrentHp;
    }
    public float GetCurrentMP()
    {
        return CurrentMp;
    }

    protected void Update()
    {
        float temp = 0;

        //HP部分
        temp = S.S[K.HPRegeneration].Get() * Time.deltaTime;
        if (gameObject.CompareTag("Player") && POEStatics.PlayerController.GetIsMoving()) { temp *= S.S[K.HPRegenerationMultiplierWhileMoving].Get(); }
        if (gameObject.CompareTag("Player") && !POEStatics.PlayerController.GetIsMoving()) { temp *= S.S[K.HPRegenerationMultiplierWhileNotMoving].Get(); }
        CurrentHp += temp;
        if (CurrentHp >= S.S[K.MaxHP].Get()) { CurrentHp = S.S[K.MaxHP].Get(); }

        //MP部分
        temp = S.S[K.MPRegeneration].Get() * Time.deltaTime;
        if (gameObject.CompareTag("Player") && POEStatics.PlayerController.GetIsMoving()) { temp *= S.S[K.MPRegenerationMultiplierWhileMoving].Get(); }
        if (gameObject.CompareTag("Player") && !POEStatics.PlayerController.GetIsMoving()) { temp *= S.S[K.MPRegenerationMultiplierWhileNotMoving].Get(); }
        CurrentMp += temp;
        if (CurrentMp >= S.S[K.MaxMP].Get()) { CurrentMp = S.S[K.MaxMP].Get(); }
    }

    public void SetHPbyPercent(float percent)
    {
        if (percent < 0)
        {
            Debug.LogErrorFormat("【{0} - {1}】：不应该传入小于0的参数！", this.GetType().ToString(), gameObject.name);
            return;
        }
        if (percent > 1)
        {
            Debug.LogErrorFormat("【{0} - {1}】：不应该传入大于1的参数！", this.GetType().ToString(), gameObject.name);
            return;
        }
        CurrentHp = S.S[K.MaxHP].Get() * percent;
    }
    public void SetMPbyPercent(float percent)
    {
        if (percent < 0)
        {
            Debug.LogErrorFormat("【{0} - {1}】：不应该传入小于0的参数！", this.GetType().ToString(), gameObject.name);
            return;
        }
        if (percent > 1)
        {
            Debug.LogErrorFormat("【{0} - {1}】：不应该传入大于1的参数！", this.GetType().ToString(), gameObject.name);
            return;
        }
        CurrentMp = S.S[K.MaxMP].Get() * percent;
    }

    protected void TakeDamage(POEStatus Source)
    {
        float physicaldamage = 0;
        physicaldamage = Source.S[K.BulletPhysicalDamage].Get();
        physicaldamage *= Source.S[K.BulletDamageMultiplier].Get();
        physicaldamage *= Source.S[K.GlobalDamageMultiplier].Get();

        float firedamage = 0;
        firedamage = Source.S[K.BulletFireDamage].Get();
        firedamage *= Source.S[K.BulletDamageMultiplier].Get();
        firedamage *= Source.S[K.GlobalDamageMultiplier].Get();

        float colddamage = 0;
        colddamage = Source.S[K.BulletColdDamage].Get();
        colddamage *= Source.S[K.BulletDamageMultiplier].Get();
        colddamage *= Source.S[K.GlobalDamageMultiplier].Get();

        float lightingdamage = 0;
        lightingdamage = Source.S[K.BulletLightinglDamage].Get();
        lightingdamage *= Source.S[K.BulletDamageMultiplier].Get();
        lightingdamage *= Source.S[K.GlobalDamageMultiplier].Get();

        TakeDamage(physicaldamage, firedamage, colddamage, lightingdamage);
    }

    protected void TakeDamage(float physicaldamage, float firedamage, float colddamage, float lightingdamage)
    {
        if (physicaldamage < 0 || firedamage < 0 || colddamage < 0 || lightingdamage < 0) //等于0的如果不让传，timescale等于0的情况下会有问题
        {
            Debug.LogErrorFormat("【{0} - {1}】：不应该传入小于0的参数！", this.GetType().ToString(), gameObject.name);
            return;
        }
        CurrentHp -= physicaldamage;
        CurrentHp -= firedamage;
        CurrentHp -= colddamage;
        CurrentHp -= lightingdamage;
    }

    public void LoseMP(float lose)
    {
        if (lose <= 0)
        {
            Debug.LogErrorFormat("【{0} - {1}】：不应该传入小于等于0的参数！", this.GetType().ToString(), gameObject.name);
            return;
        }
        CurrentMp -= lose;
    }
}
