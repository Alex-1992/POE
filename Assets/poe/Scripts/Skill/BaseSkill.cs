using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : BaseObject
{

    protected string _skillName;
    protected int _skillType;
    protected float _skillCD;

    private BaseEffect _skillEffect;

    public void CastSkills()
    {
        if (_skillEffect)
        {

        }
    }
}
