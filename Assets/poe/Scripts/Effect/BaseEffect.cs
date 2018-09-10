using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : BaseObject
{

    private string _effectType;
    protected BaseUnit _caster;   //施法者
    protected BaseUnit _source;   //源单位
    protected BaseUnit _target;   //当前目标
    protected BaseUnit _selectTarget;   //选择目标
    protected BaseEffect _casterEffect;   //施法效果
    protected BaseEffect _sourceEffect;   //源效果

    //当前为第几代
    protected int _generation = 0;    //变量getset
    public int Teneration
    {
        set { _generation = value; }
        get { return _generation; }
    }

    public BaseEffect()
    {
    }

    //启动效果
    public void StartUp()
    {

    }
}
