using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : BaseObject
{

    private string _abilityId;
    private BaseUnit _father;
    private float _baseValue;            //基础值
    private float _additionValue;        //基础加成
    private float _percentageValue;      //百分比加成
    private float _moreValue;            //更多加成
    private float _finalValue;           //最终值
    Dictionary<string, object> _attributeValue = new Dictionary<string, object>();

    public BaseAbility(string abilityId, float value, BaseUnit unit)
    {
        _abilityId = abilityId;
        _baseValue = value;
        _father = unit;
    }

    //基础属性
    public float BaseValue
    {
        get { return _baseValue; }
        set
        {
            _baseValue = value;
            ComputationalAttributes();
            _father.Emit(EventType.ABILITY_CHANGE, this);
        }
    }
    //基础加成
    public float AdditionValue
    {
        get { return _additionValue; }
        set
        {
            _additionValue = value;
            ComputationalAttributes();
            _father.Emit(EventType.ABILITY_CHANGE, this);
        }
    }
    //百分比加成
    public float PercentageValue
    {
        get { return _percentageValue; }
        set
        {
            _percentageValue = value;
            ComputationalAttributes();
            _father.Emit(EventType.ABILITY_CHANGE, this);
        }
    }
    //更多加成
    public float MoreValue
    {
        get { return _moreValue; }
        set
        {
            _moreValue = value;
            ComputationalAttributes();
            _father.Emit(EventType.ABILITY_CHANGE, this);
        }
    }
    //最终值
    public float FinalValue
    {
        get { return _finalValue; }
    }
    //计算最终值
    private void ComputationalAttributes()
    {
        _finalValue = ((_baseValue + _additionValue) * (1 + _percentageValue)) * _moreValue;
    }
}
