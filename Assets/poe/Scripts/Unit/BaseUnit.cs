using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : BaseObject
{

    //属性表
    public Dictionary<string, BaseAbility> _abilityList = new Dictionary<string, BaseAbility>();
    //词缀表
    public List<string> _affixList = new List<string>();

    // Use this for initialization
    void Start () {
        //添加测试属性
        _abilityList.Add("HP", new BaseAbility("HP", 100, this));
        _abilityList.Add("MAXHP", new BaseAbility("MAXHP", 100, this));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
