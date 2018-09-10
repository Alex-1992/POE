using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamage : BaseEffect
{
    //================属性配置
    private float _damageValue;
    public float _damageWeapon;    //享受的武器加成%
    public List<string> _adductAffix = new List<string>();   //享受的词缀加成
    public List<string> _preEffect = new List<string>();   //伤害前效果ID
    public List<string> _afferEffect = new List<string>();   //伤害后效果ID
    //================局部变量
    private float completeDameage;  //伤害值(减免前)
    private float actualDamage;     //伤害值(减免后)
    //================私有方法
    private void DamageTreatment()
    {
        
    }
    //================公有方法
    public new void StartUp()
    {

    }
}
